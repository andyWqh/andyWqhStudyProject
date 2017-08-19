using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HelpClassLib.Web
{
     /// <summary>
     /// Json帮助类
     /// </summary>
    public class JsonHelperClass
    {
       static JavaScriptSerializer myJson = new JavaScriptSerializer();

        #region Json与DataTable转换方法
        /// <summary>
        /// 对象转换成Json
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <returns>Json格式的字符串</returns>
        public static string ObjectToJson(object obj)
        {
            string jsonStr = string.Empty;
            if (obj == null)
            {
                jsonStr = string.Empty;
            }
            else
            {
                try
                {
                    jsonStr = myJson.Serialize(obj);
                }
                catch (Exception)
                {

                }
            }
            return jsonStr;
        }

        /// <summary>
        /// 数据表转键值对集合
        /// 把DataTable转成List集合，存每一行的记录
        /// 集合中放的是键值对字典，存每一列
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>哈希表数值</returns>
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                    list.Add(dic);
                }
                return list;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// 数据集转键值对数组字典
        /// </summary>
        /// <param name="ds">数据集</param>
        /// <returns>键值对数组字典</returns>
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToJson(DataSet ds)
        {
            if (ds != null && ds.Tables != null && ds.Tables[0].Rows.Count > 0)
            {
                Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();
                foreach (DataTable dt in ds.Tables)
                {
                    result.Add(dt.TableName, DataTableToList(dt));
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 数据表转换成Json数据格式
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>Json格式字符串</returns>
        public static string DataTableToJson(DataTable dt)
        {
            return ObjectToJson(DataTableToList(dt));
        }

        /// <summary>
        /// Json文本转换成对象,泛型方法
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="jsonText">Json文本</param>
        /// <returns>指定类型的对象</returns>
        public static T JsonToObject<T>(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                return default(T);
            }
            return myJson.Deserialize<T>(jsonText);
        }

        /// <summary>
        /// 将Json文本转换成数据表数据
        /// </summary>
        /// <param name="jsonText">Json文本</param>
        /// <returns>数据表字典</returns>
        public static Dictionary<string, List<Dictionary<string, object>>> JsonToDataTable(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }
            else
            {
                return JsonToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
            }
           
        }

        /// <summary>
        /// Json文本转换成数据行
        /// </summary>
        /// <param name="jsonText">Json文本</param>
        /// <returns>数据行字典</returns>
        public static Dictionary<string, object> JsonToDataRow(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }
            else
            {
                return JsonToObject<Dictionary<string, object>>(jsonText);
            }
        } 
        #endregion

        #region Json序列化与反序列化
        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="T">待转换数据类型</typeparam>
        /// <param name="obj">待转换数据对象</param>
        /// <returns>Json格式字符串</returns>
        public static string JsonSerializer<T>( T obj)
        {
            string jsonStr = string.Empty;
            if (obj == null)
            {
                return jsonStr;
            }
            else
            {
                try
                {
                    DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(T));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        dcjs.WriteObject(ms, obj);
                        jsonStr = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
                catch (Exception)
                {
                    jsonStr = string.Empty;
                }
                return jsonStr;
            }
        }

        /// <summary>
        /// 将DataTable序列化成Json字符串
        /// </summary>
        /// <param name="dt">dataTable数据集</param>
        /// <returns>Json格式字符串</returns>
        public static string DataTableChangeToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "\"\"";
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc] + "");
                }
                list.Add(result);
            }
            return myJson.Serialize(list);
        }

        /// <summary>
        /// 将对象序列化成json格式
        /// </summary>
        /// <param name="obj">数据集对象</param>
        /// <returns>json格式字符串</returns>
        public static string ObjectChangeToJson(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return myJson.Serialize(obj);
        }

        /// <summary>
        /// 将json字符串转换成对象
        /// </summary>
        /// <param name="jsonText">json字符串</param>
        /// <returns>对象</returns>
        public static object JsonChangeToObject(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }
            return myJson.DeserializeObject(jsonText);
        }


        /// <summary>
        /// 将json字符串转换成指定类型的对象
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="jsonText">json字符串</param>
        /// <returns>指定类型的对象</returns>
        public static T JsonChangeToObject<T>(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
            {
                return default(T);
            }
            return myJson.Deserialize<T>(jsonText);
        }
        #endregion

    }
}
