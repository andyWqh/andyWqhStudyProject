using System;
using System.Configuration;
using System.Data;
using System.Collections;
using Oracle.DataAccess.Client;
using System.Web;

namespace HelpClassLib.DataBase
{

    /// <summary>
    /// A helper class used to execute queries against an Oracle database
    /// </summary>
    public abstract class OraHelper
    {
        public static readonly string CONN_STRING_NON_DTC = ConfigurationManager.ConnectionStrings["andyWqh"].ConnectionString;
        //public static readonly string CONN_STRING_NON_DTC = "Data Source=shenzhenfoctest;Persist Security Info=True;User ID=shenzhenfoc;Password=yuabc;Unicode=True";


        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a database query which does not include a select
        /// </summary>
        /// <param name="connString">Connection string to database</param>
        /// <param name="cmdType">Command type either stored procedure or SQL</param>
        /// <param name="cmdText">Acutall SQL Command</param>
        /// <param name="cmdParms">Parameters to bind to the command</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            int val = 0;
            // Create a new Oracle command
            OracleCommand cmd = new OracleCommand();
            //Create a connection
            OracleConnection conn = new OracleConnection(connString);
            //Prepare the command
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                //Execute the command
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return val;
        }

        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against an existing database transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing database transaction</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or PL/SQL command</param>
        /// <param name="cmdParms">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            int val = 0;
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (trans != null)
                {
                    trans.Dispose();
                }
            }
            return val;
        }

        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or PL/SQL command</param>
        /// <param name="cmdParms">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            int val = 0;
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return val;
        }

        
        /// <summary>
        ///     执行一个SQL Command(使用隐含的ConnectString)
        /// </summary>
        /// <param name="cmdType">Command类型</param>
        ///     <param name="cmdText">Command的语句（SQL语句）</param>
        ///     <param name="cmdParms">Command的参数（SqlParameter[]数组类型）</param>
        ///     <returns>Command的返回值（受影响的行数）</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            int val = 0;
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return val;
        }

        /// <summary>
        /// 执行一个简单的查询, 只需要输入SQL语句, 一般用于更新或者删除
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sqlText)
        {
            int val = 0;
            try
            {
                val = ExecuteNonQuery(CommandType.Text, sqlText);
            }
            catch (Exception e)
            {
                throw e;
            }           
            return val;
        }

        /// <summary>
        ///     根据指定DsCommandType类型，自动生成cmd执行dataset的更新
        /// </summary>
        /// <param name="connString">ConnectString（Sql连接字符串）</param>
        ///     <param name="cmdType">Command类型</param>
        ///     <param name="dsCommandType">Enum类型</param>
        ///     <param name="cmdText">Command的语句（SQL语句）</param>
        ///     <param name="dataset">dataset</param>
        ///     <param name="tablename">表名</param>
        ///     <param name="cmdParms">Command的参数（OracleParameter[]数组类型）</param>
        ///     <returns>是否更新成功</returns>
        public static bool ExecuteNonQuery(string connString, CommandType cmdType, CommandEnum.DsCommandType dsCommandType, string cmdText, DataSet dataset, string tablename, params OracleParameter[] cmdParms)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connString);
            bool isSuccess = false;
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
            switch (dsCommandType)
            {
                case CommandEnum.DsCommandType.InsertCommand:
                    dsCommand.InsertCommand = cmd;
                    break;
                case CommandEnum.DsCommandType.UpdateCommand:
                    dsCommand.UpdateCommand = cmd;
                    break;
                case CommandEnum.DsCommandType.DeleteCommand:
                    dsCommand.DeleteCommand = cmd;
                    break;
                default: break;
            }
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                dsCommand.Update(dataset, tablename);
                if (dataset.HasErrors)
                {
                    dataset.Tables[tablename].GetErrors()[0].ClearErrors();
                    isSuccess = false;
                }
                else
                {
                    dataset.AcceptChanges();
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return isSuccess;
        }

        /// <summary>
        ///     根据指定DsCommandType类型，自动生成cmd执行dataset的更新
        /// </summary>
        /// <param name="connString">ConnectString（Sql连接字符串）</param>
        ///     <param name="cmdType">Command类型</param>
        ///     <param name="dsCommandType">Enum类型</param>
        ///     <param name="cmdText">Command的语句（SQL语句）</param>
        ///     <param name="dataset">dataset</param>
        ///     <param name="tablename">表名</param>
        ///     <param name="isContinue">出现错误是否继续更新</param>
        ///     <param name="cmdParms">Command的参数（OracleParameter[]数组类型）</param>
        ///     <returns>返回错误条数</returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, CommandEnum.DsCommandType dsCommandType, string cmdText, DataSet dataset, string tablename, bool isContinue, params OracleParameter[] cmdParms)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            int errorCount = 0;
            if (isContinue)
            {
                dsCommand.ContinueUpdateOnError = true;
            }

            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connString);
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
            switch (dsCommandType)
            {
                case CommandEnum.DsCommandType.InsertCommand:
                    dsCommand.InsertCommand = cmd;
                    break;
                case CommandEnum.DsCommandType.UpdateCommand:
                    dsCommand.UpdateCommand = cmd;
                    break;
                case CommandEnum.DsCommandType.DeleteCommand:
                    dsCommand.DeleteCommand = cmd;
                    break;
                default: break;
            }
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                dsCommand.Update(dataset, tablename);
                if (dataset.HasErrors)
                {
                    errorCount = dataset.Tables[tablename].GetErrors().Length;
                }
                else
                {
                    dataset.AcceptChanges();
                    errorCount = 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return errorCount;
        }


        /// <summary>
        ///     更新一个记录集(使用connString)
        /// </summary>
        ///     <param name="connString">ConnectString（Sql连接字符串）</param>
        ///     <param name="cmdInsertType">commandInsert类型</param>
        ///     <param name="cmdInsertText">SQL语句（Insert）</param>
        ///     <param name="cmdUpdateType">commandUpdate类型</param>
        ///     <param name="cmdUpdateText">SQL语句（Update）</param>
        ///     <param name="cmdInsertType">commandDelete类型</param>
        ///     <param name="cmdDeleteText">SQL语句（Delete）</param>
        ///     <param name="cmdInsertParms">InsertCommand参数</param>
        ///     <param name="cmdUpdateParms">UpdateCommand参数</param>
        ///     <param name="cmdDeleteParms">DeleteCommand参数</param>
        ///     <param name="dataset">dataset</param>
        ///     <param name="tablename">表名</param>
        ///     <returns>是否更新成功</returns>  
        public static bool UpdateDataset(string connString, CommandType cmdInsertType, string cmdInsertText, CommandType cmdUpdateType, string cmdUpdateText, CommandType cmdDeleteType, string cmdDeleteText, OracleParameter[] cmdInsertParms, OracleParameter[] cmdUpdateParms, OracleParameter[] cmdDeleteParms, DataSet dataset, string tablename)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            OracleConnection conn = new OracleConnection(connString);
            bool isSuccess = false;
            if (cmdInsertText != String.Empty)
            {
                OracleCommand cmdInsert = new OracleCommand();
                cmdInsert.Connection = conn;
                cmdInsert.CommandText = cmdInsertText;
                cmdInsert.CommandType = cmdInsertType;
                if (cmdInsertParms != null)
                {
                    foreach (OracleParameter parm in cmdInsertParms)
                        cmdInsert.Parameters.Add(parm);
                }
                dsCommand.InsertCommand = cmdInsert;
            }
            if (cmdUpdateText != String.Empty)
            {
                OracleCommand cmdUpdate = new OracleCommand();
                cmdUpdate.Connection = conn;
                cmdUpdate.CommandText = cmdUpdateText;
                cmdUpdate.CommandType = cmdUpdateType;
                if (cmdUpdateParms != null)
                {
                    foreach (OracleParameter parm in cmdUpdateParms)
                        cmdUpdate.Parameters.Add(parm);
                }
                dsCommand.UpdateCommand = cmdUpdate;
            }
            if (cmdDeleteText != String.Empty)
            {
                OracleCommand cmdDelete = new OracleCommand();
                cmdDelete.Connection = conn;
                cmdDelete.CommandText = cmdDeleteText;
                cmdDelete.CommandType = cmdDeleteType;
                if (cmdDeleteParms != null)
                {
                    foreach (OracleParameter parm in cmdDeleteParms)
                        cmdDelete.Parameters.Add(parm);
                }
                dsCommand.DeleteCommand = cmdDelete;
            }
            if (cmdInsertText == String.Empty && cmdUpdateText == String.Empty && cmdDeleteText == String.Empty)
            {
                //					OracleCommandBuilder scb = new OracleCommandBuilder(dsCommand);
                isSuccess = false;
            }
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                dsCommand.Update(dataset, tablename);
                if (dataset.HasErrors)
                {
                    dataset.Tables[tablename].GetErrors()[0].ClearErrors();
                    isSuccess = false;
                }
                else
                {
                    dataset.AcceptChanges();
                    isSuccess = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return isSuccess;
        }

        
        public static OracleDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            //Create the command and connection
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connString);
            OracleDataReader rdr = null;
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }            
            return rdr;
        }

        /// <summary>
        /// 获取一个OracleDataReader(使用connString), 使用缺省的ConnectionString
        /// </summary>
        ///     <param name="cmdType">类型</param>
        ///     <param name="cmdText">Command的语句(select语句)</param>
        ///     <param name="cmdParms">Command的参数</param>
        ///  <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            OracleDataReader rdr = null;
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            return rdr;
        }

        /// <summary>
        /// 获取一个OracleDataReader(使用connString), 使用缺省的ConnectionString
        /// </summary>
        ///     <param name="conn">数据库连接</param>
        ///     <param name="cmdType">类型</param>
        ///     <param name="cmdText">Command的语句(select语句)</param>
        ///     <param name="cmdParms">Command的参数</param>
        ///  <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataReader rdr = null;
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            return rdr;
        }


        /// <summary>
        ///  get a OracleDataReader, use default ConnectionString
        /// </summary>
        /// <param name="cmdtxt">command</param>
        /// <returns></returns>
        public OracleDataReader ExecuteReader(string cmdtxt)
        {
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            OracleCommand cmd = new OracleCommand(cmdtxt, conn);
            OracleDataReader rdr = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw e;
            }
            return rdr;
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return val;
        }

        /// <summary>
        /// Execute an OracleCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="connString">a valid connection string for a OracleConnection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or PL/SQL command</param>
        /// <param name="cmdParms">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            object val = null;
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connString);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return val;
        }

        /// <summary>
        /// Execute an OracleCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or PL/SQL command</param>
        /// <param name="cmdParms">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(OracleConnection conn, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = null;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    Open(conn);
                    //conn.Open();
                }
                val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return val;
        }

        ///#region 填充DataSet


        /// <summary>
        ///     将数据填充到DataSet中(默认connString)
        /// </summary>
        ///     <param name="cmdType">类型</param>
        ///     <param name="cmdText">Command的语句</param>
        ///     <param name="tablename">表名</param>
        ///     <param name="cmdParms">Command的参数</param>
        public static void FillData(CommandType cmdType, string cmdText, DataSet dataset, string tablename, params OracleParameter[] cmdParms)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand();
            dsCommand.SelectCommand = cmd;
            //dsCommand.TableMappings.Add("Table",tablename);
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                dsCommand.Fill(dataset, tablename);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        /// <summary>
        ///     将数据填充到DataSet中(默认connString)
        /// </summary>
        ///     <param name="cmdType">类型</param>
        ///     <param name="cmdText">Command的语句</param>
        ///     <param name="tablename">表名</param>
        ///     <param name="cmdParms">Command的参数</param>
        public static void FillData(CommandType cmdType, string cmdText, DataSet dataset, string tablename, int startRecord, int maxRecords, params OracleParameter[] cmdParms)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand();
            dsCommand.SelectCommand = cmd;
            //dsCommand.TableMappings.Add("Table",tablename);
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                dsCommand.Fill(dataset,startRecord,maxRecords,tablename);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        /// <summary>
        ///     将数据填充到DataSet中(使用connString + OracleParameterCollection)
        /// </summary>
        ///     <param name="connString">ConnectString</param>
        ///     <param name="cmdType">类型</param>
        ///     <param name="cmdText">Command的语句</param>
        ///     <param name="tablename">表名</param>
        ///     <param name="cmdParms">Command的参数(OracleParameterCollection)</param>
        public static void FillData(string connString, CommandType cmdType, string cmdText, DataSet dataset, string tablename, OracleParameterCollection cmdParms)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand();
            dsCommand.SelectCommand = cmd;
            dsCommand.TableMappings.Add("Table", tablename);
            OracleConnection conn = new OracleConnection(connString);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                dsCommand.Fill(dataset);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        ///     将数据填充到DataSet中(默认connString)
        /// </summary>
        ///     <param name="cmdType">类型</param>
        ///     <param name="cmdText">Command的语句</param>
        ///     <param name="cmdParms">Command的参数</param>
        public static void FillProc(CommandType cmdType, string cmdText, DataSet dataset, params OracleParameter[] cmdParms)
        {
            OracleDataAdapter dsCommand = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand();
            dsCommand.SelectCommand = cmd;
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            try
            {
                dsCommand.Fill(dataset);
                cmd.Parameters.Clear();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (dsCommand != null)
                    dsCommand.Dispose();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        ///#endregion



        /// <summary>
        /// Add a set of parameters to the cached
        /// </summary>
        /// <param name="cacheKey">Key value to look up the parameters</param>
        /// <param name="cmdParms">Actual parameters to cached</param>
        public static void CacheParameters(string cacheKey, params OracleParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Fetch parameters from the cache
        /// </summary>
        /// <param name="cacheKey">Key to look up the parameters</param>
        /// <returns></returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];
            if (cachedParms == null)
            {
                return null;
            }
            // If the parameters are in the cache
            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];
            // return a copy of the parameters
            for (int i = 0, j = cachedParms.Length; i < j; i++)
            {
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();
            }
            return clonedParms;
        }

        /// <summary>
        ///     准备一个Command(使用OracleParameter[]数组)
        /// </summary>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameterCollection cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            cmd.Parameters.Clear();
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
        }


        /// <summary>
        /// Internal function to prepare a command for execution by the database
        /// </summary>
        /// <param name="cmd">Existing command object</param>
        /// <param name="conn">Database connection object</param>
        /// <param name="trans">Optional transaction object</param>
        /// <param name="cmdType">Command type, e.g. stored procedure</param>
        /// <param name="cmdText">Command test</param>
        /// <param name="cmdParms">Parameters for the command</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] cmdParms)
        {
            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            //Bind it to the transaction if it exists
            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            // Bind the parameters passed in
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
        }

        private static void Open(OracleConnection conn)
        {
            //插入监控脚本
            //OracleCommand cmd = new OracleCommand();
            //string userId = HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : "匿名";
            //string strSql = "select 'GuardAppUser:" + userId + "' from dual ";
            //cmd.Connection = conn;
            //cmd.CommandText = strSql;
            //cmd.CommandType = CommandType.Text;
            conn.Open();
            //cmd.ExecuteNonQuery();
        }
        private static void Close(OracleConnection conn)
        {
            conn.Close();
        }

        /// <summary>
        /// add by andyWqh 2016-11-29
        /// </summary>
        /// <param name="sqlConnectionString">连接字符串</param>
        /// <returns></returns>
        public static OracleConnection GetSqlConnection()
        {
            //SqlConnection conn = new SqlConnection(sqlConnectionString);
            //conn.Open();
            //return conn;
            OracleConnection conn = new OracleConnection(CONN_STRING_NON_DTC);
            if (conn != null)
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            return conn;
        }
    }

}
