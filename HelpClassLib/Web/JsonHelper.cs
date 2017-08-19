using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data;

namespace HelpClassLib.Web
{
    public class JsonHelper
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        /// <summary>
        /// DataTable数据表转换成Json格式数据
        /// </summary>
        /// <param name="table">带转换的数据表格</param>
        /// <param name="jsName">DataTable数据总记录参数名称</param>
        /// <returns>Json格式数据</returns>
        public static string CreateJsonDataByTable(DataTable table,string jsName)
        {
            StringBuilder json = new StringBuilder("{\"" + jsName + "\":[");
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    json.Append("{");
                    foreach (DataColumn column in table.Columns)
                    {
                        json.Append("\"" + column.ColumnName + "\":\"" + row[column.ColumnName].ToString() + "\",");
                    }
                    json.Remove(json.Length - 1, 1);
                    json.Append("},");
                }
                json.Remove(json.Length - 1, 1);
            }
            json.Append("]}");
            return json.ToString();
        }

        /// <summary>
        /// 将dataTable数据转换成json格式
        /// </summary>
        /// <param name="dt">dataTable数据表</param>
        /// <param name="displayCount">是否显示总记录数</param>
        /// <param name="totalCount">显示总记录数</param>
        /// <param name="jsonName">显示总记录数参数名称</param>
        /// <param name="rowName">行记录参数名称</param>
        /// <returns>json格式</returns>
        public static string CreateJsonParameters(DataTable dt, bool displayCount, int totalCount,string jsonName,string rowName)
        {
            StringBuilder jsonStr = new StringBuilder();
            if (dt != null)
            {
                jsonStr.Append("{");
                if (displayCount)
                {
                    jsonStr.Append("\"" + jsonName + "\":");
                    jsonStr.Append(totalCount);
                    jsonStr.Append(",");
                }
                jsonStr.Append("\"" + rowName + "\":[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonStr.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            if (dt.Columns[j].DataType == typeof(bool))
                            {
                                jsonStr.Append( "\""+dt.Columns[j].ColumnName.ToLower() + "\":" +
                                                 dt.Rows[i][j].ToString().ToLower() + ",");
                            }
                            else if (dt.Columns[j].DataType == typeof(string))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" +
                                                  dt.Rows[i][j].ToString().Replace("\"", "\\\"") + "\",");
                            }
                            else
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" + dt.Rows[i][j] + "\",");
                            }
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            //if (dt.Rows[i][j] == DBNull.Value) continue;
                            if (dt.Columns[j].DataType == typeof(bool))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" +
                                                  dt.Rows[i][j].ToString().ToLower());
                            }
                            else if (dt.Columns[j].DataType == typeof(string))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" +
                                                  dt.Rows[i][j].ToString().Replace("\"", "\\\"") + "\"");
                            }
                            else
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" + dt.Rows[i][j] + "\"");
                            }
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        jsonStr.Append("} ");
                    }
                    else
                    {
                        jsonStr.Append("}, ");
                    }
                }
                jsonStr.Append("]");
                jsonStr.Append("}");
                return jsonStr.ToString().Replace("\n", "");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将table表的数据转换成json格式
        /// </summary>
        /// <param name="dt">table数据表</param>
        /// <param name="name">json格式数据参数名称</param>
        /// <returns>json格式数据</returns>
        public static string DataTableToJson(DataTable dt, string name)
        {
            StringBuilder jsonStr = new StringBuilder("{\"" + name + "\":[");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    jsonStr.Append("{");
                    foreach (DataColumn cloumn in dt.Columns)
                    {
                        jsonStr.Append("\"" + cloumn.ColumnName + "\":\"" + row[cloumn.ColumnName] + "\",");
                    }
                    jsonStr.Remove(jsonStr.Length - 1, 1);
                    jsonStr.Append("},");
                }
                jsonStr.Remove(jsonStr.Length - 1, 1);
            }
            jsonStr.Append("]}");
            return jsonStr.ToString();
        }

        /// <summary>
        /// 将table表的数据转换成json格式
        /// </summary>
        /// <param name="dt">table数据表</param>
        /// <param name="name">json格式数据参数名称</param>
        /// <returns>json格式数据</returns>
        public static string DataTableToJsonData(DataTable dt)
        {
            StringBuilder jsonStr = new StringBuilder("[");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    jsonStr.Append("{");
                    foreach (DataColumn cloumn in dt.Columns)
                    {
                        jsonStr.Append("\"" + cloumn.ColumnName.ToLower() + "\":\"" + row[cloumn.ColumnName].ToString() + "\",");
                    }
                    jsonStr.Remove(jsonStr.Length - 1, 1);
                    jsonStr.Append("},");
                }
                jsonStr.Remove(jsonStr.Length - 1, 1);
            }
            jsonStr.Append("]");
            return jsonStr.ToString();
        }


        /// <summary>
        /// 将table表的数据转换成json格式
        /// </summary>
        /// <param name="dt">table数据表</param>
        /// <param name="name">json格式数据参数名称</param>
        /// <returns>json格式数据</returns>
        public static string DataTableToJsonData(DataTable dt, string name)
        {
            StringBuilder jsonStr = new StringBuilder("{\"" + name + "\":[");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    jsonStr.Append("{");
                    foreach (DataColumn cloumn in dt.Columns)
                    {
                        jsonStr.Append("\"" + cloumn.ColumnName.ToLower() + "\":\"" + ReplaceSpecialCharacters(row[cloumn.ColumnName].ToString()) + "\",");
                    }
                    jsonStr.Remove(jsonStr.Length - 1, 1);
                    jsonStr.Append("},");
                }
                jsonStr.Remove(jsonStr.Length - 1, 1);
            }
            jsonStr.Append("]}");
            return jsonStr.ToString();
        }

        /// <summary>
        /// 将dataTable数据转换成json格式(新增方法，处理字符串中含有特殊字符转化json时出错问题)
        /// </summary>
        /// <param name="dt">dataTable数据表</param>
        /// <param name="displayCount">是否显示总记录数</param>
        /// <param name="totalCount">显示总记录数</param>
        /// <param name="jsonName">显示总记录数参数名称</param>
        /// <param name="rowName">行记录参数名称</param>
        /// <returns>json格式</returns>
        public static string DataToJsonData(DataTable dt, bool displayCount, int totalCount, string jsonName, string rowName)
        {
            StringBuilder jsonStr = new StringBuilder();
            if (dt != null)
            {
                jsonStr.Append("{");
                if (displayCount)
                {
                    jsonStr.Append("\"" + jsonName + "\":");
                    jsonStr.Append(totalCount);
                    jsonStr.Append(",");
                }
                jsonStr.Append("\"" + rowName + "\":[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonStr.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            if (dt.Columns[j].DataType == typeof(bool))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" +
                                                 dt.Rows[i][j].ToString().ToLower() + ",");
                            }
                            else if (dt.Columns[j].DataType == typeof(string))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" +
                                                  ReplaceSpecialCharacters(dt.Rows[i][j].ToString()) + "\",");
                            }
                            else
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" + dt.Rows[i][j] + "\",");
                            }
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            //if (dt.Rows[i][j] == DBNull.Value) continue;
                            if (dt.Columns[j].DataType == typeof(bool))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" +
                                                  dt.Rows[i][j].ToString().ToLower());
                            }
                            else if (dt.Columns[j].DataType == typeof(string))
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" + ReplaceSpecialCharacters(dt.Rows[i][j].ToString()) + "\"");
                            }
                            else
                            {
                                jsonStr.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\":" + "\"" + dt.Rows[i][j] + "\"");
                            }
                        }
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        jsonStr.Append("} ");
                    }
                    else
                    {
                        jsonStr.Append("}, ");
                    }
                }
                jsonStr.Append("]");
                jsonStr.Append("}");
                return jsonStr.ToString().Replace("\n", "");
            }
            else
            {
                return null;
            } 
        }

        /// <summary>
        /// 将字符串中的特殊字符进行转义
        /// </summary>
        /// <param name="orgData">字符串</param>
        /// <returns></returns>
        public static string ReplaceSpecialCharacters(string orgData)
        {
            orgData = orgData.Replace(">", "&gt;").
                              Replace("<", "&lt;").
                              Replace(" ", "&nbsp;").
                              Replace("\"", "&quot;").
                              Replace("\'", "\\\'").
                              Replace("\\", "\\\\").
                              Replace("\n", "<br/>").
                              Replace("\r", " ").
                              Replace("\t", " ");
            return orgData;
        }
    }
}
