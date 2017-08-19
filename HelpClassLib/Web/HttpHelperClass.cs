using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;

namespace HelpClassLib.Web
{
    /// <summary>
    /// 文件上传下载帮助类
    /// </summary>
    public class HttpHelperClass
    {
        #region Get和Post方式上传下载
        private static HttpHelperClass helperClass;

        CookieCollection cookies = new CookieCollection();
        /// <summary>
        /// 创建实例对象
        /// </summary>
        public static HttpHelperClass HelperClass
        {
            get
            {
                if (helperClass == null)
                {
                    helperClass = new HttpHelperClass();
                }

                return helperClass;
            }
        }

        /// <summary>
        /// WebRequest类的http代理设置
        /// </summary>
        public WebProxy Proxy
        {
            get;
            set;
        }

        public bool Debug { get; set; }

        public CookieCollection Cookies
        {
            get
            {
                return cookies;
            }
        }

        public void ClearCookies()
        {
            cookies = new CookieCollection();
        }

        /// <summary>
        /// 设置默认的浏览器内核解析
        /// </summary>
        private static readonly string DefaultUserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";

        /// <summary>
        /// 创建Get方式的http请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器的信息</param>
        /// <param name="cookies">随同http请求发送的Cookies信息，如果不要身份验证可以为空</param>
        /// <param name="referer">http标头的值</param>
        /// <param name="headers">http标头键值对</param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public HttpWebResponse CreateGetHttpResponse(string url, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-from-urlencoded")
        {
            if (Debug)
            {
                Console.Write("Start Get Utl:{0}", url);
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            //定义http请求对象
            HttpWebRequest request;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (Proxy != null)
            {
                request.Proxy = this.Proxy;
            }

            request.Method = "GET";
            request.Headers["Pragma"] = "no-cache";
            request.Accept = "text/html,application/xhtml+xml,*/*";
            request.Headers["Accept-Language"] = "en-US,en;q= 0.5";
            request.ContentType = contentType;
            request.UserAgent = DefaultUserAgent;
            request.Referer = referer;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value * 1000;
            }

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            else
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(this.Cookies);
            }

            var v = request.GetResponse() as HttpWebResponse;

            Cookies.Add(request.CookieContainer.GetCookies(new Uri("http://" + new Uri(url).Host)));
            Cookies.Add(request.CookieContainer.GetCookies(new Uri("https://" + new Uri(url).Host)));
            Cookies.Add(v.Cookies);

            if (Debug)
            {
                Console.Write("ok");
            }

            return v;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public HttpWebResponse CreatePostHttpResponse(string url, string parameters, Encoding requestEncoding, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            if (Debug)
            {
                Console.Write("Start Post Url:{0},parameter{1}:", url, parameters);
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            HttpWebRequest request = null;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (Proxy != null)
            {
                request.Proxy = Proxy;
            }

            request.Method = "POST";
            request.Headers.Add("Accept-Language", "zh-CN,en-GB;q=0.5");
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.Referer = Referer;
            request.Headers["Accept-Language"] = "en-US,en;q=0.5";
            request.UserAgent = DefaultUserAgent;
            request.ContentType = contentType;
            request.Headers["Pragma"] = "no-cache";

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            else
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(Cookies);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value * 1000;
            }
            request.Expect = string.Empty;
            //如果需要post数据
            if (!string.IsNullOrEmpty(parameters))
            {
                byte[] data = requestEncoding.GetBytes(parameters);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            var v = request.GetResponse() as HttpWebResponse;
            Cookies.Add(request.CookieContainer.GetCookies(new Uri("http://" + new Uri(url).Host)));
            Cookies.Add(request.CookieContainer.GetCookies(new Uri("https://" + new Uri(url).Host)));
            Cookies.Add(v.Cookies);
            if (Debug)
            {
                Console.WriteLine("OK");
            }
            return v;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public HttpWebResponse CreatePostFileHttpResponse(string url, string filePath, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            if (Debug)
            {
                Console.Write("Start Post Url:{0}  ", url);

            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (Proxy != null)
            {
                request.Proxy = Proxy;
            }
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
            request.Referer = Referer;
            request.Headers["Accept-Language"] = "en-US,en;q=0.5";
            request.UserAgent = DefaultUserAgent;
            request.ContentType = contentType;
            request.Headers["Pragma"] = "no-cache";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            else
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(Cookies);
            }


            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value * 1000;
            }

            request.Expect = string.Empty;

            //如果需要POST数据  
            if (!string.IsNullOrEmpty(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader r = new BinaryReader(fs);

                    //时间戳 
                    string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
                    byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

                    //请求头部信息 
                    StringBuilder sb = new StringBuilder();
                    sb.Append("--");
                    sb.Append(strBoundary);
                    sb.Append("\r\n");
                    sb.Append("Content-Disposition: form-data; name=\"");
                    sb.Append("file");
                    sb.Append("\"; filename=\"");
                    sb.Append(fs.Name);
                    sb.Append("\"");
                    sb.Append("\r\n");
                    sb.Append("Content-Type: ");
                    sb.Append("application/octet-stream");
                    sb.Append("\r\n");
                    sb.Append("\r\n");
                    string strPostHeader = sb.ToString();
                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);


                    request.ContentType = "multipart/form-data; boundary=" + strBoundary;
                    long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;

                    request.ContentLength = length;

                    //开始上传时间 
                    DateTime startTime = DateTime.Now;

                    byte[] filecontent = new byte[fs.Length];

                    fs.Read(filecontent, 0, filecontent.Length);

                    using (Stream stream = request.GetRequestStream())
                    {

                        //发送请求头部消息 
                        stream.Write(postHeaderBytes, 0, postHeaderBytes.Length);

                        stream.Write(filecontent, 0, filecontent.Length);

                        //添加尾部的时间戳 
                        stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    }

                }
            }
            var v = request.GetResponse() as HttpWebResponse;

            Cookies.Add(request.CookieContainer.GetCookies(new Uri("http://" + new Uri(url).Host)));
            Cookies.Add(request.CookieContainer.GetCookies(new Uri("https://" + new Uri(url).Host)));

            Cookies.Add(v.Cookies);

            if (Debug)
            {
                Console.WriteLine("OK");
            }
            return v;
        }


        public static string CraeteParameter(IDictionary<string, string> parameters)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (string key in parameters.Keys)
            {
                buffer.AppendFormat("&{0}={1}", key, parameters[key]);
            }
            return buffer.ToString().TrimStart('&');
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }



        public string Post(string url, IDictionary<string, string> parameters, Encoding requestEncoding, Encoding responseEncoding, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebResponse response = CreatePostHttpResponse(url, parameters + "", requestEncoding, timeout, userAgent, cookies, Referer, headers, contentType);

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public T Post<T>(string url, IDictionary<string, string> parameters, Encoding requestEncoding, Encoding responseEncoding, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            return JsonConvert.DeserializeObject<T>(Post(url, parameters, requestEncoding, responseEncoding, timeout, userAgent, cookies, Referer, headers, contentType));
        }


        public string Post(string url, string parameters, Encoding requestEncoding, Encoding responseEncoding, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebResponse response = CreatePostHttpResponse(url, parameters, requestEncoding, timeout, userAgent, cookies, Referer, headers, contentType);

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T Post<T>(string url, string parameters, Encoding requestEncoding, Encoding responseEncoding, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            return JsonConvert.DeserializeObject<T>(Post(url, parameters, requestEncoding, responseEncoding, timeout, userAgent, cookies, Referer, headers, contentType));
        }

        public string PostFile(string url, string filePath, Encoding responseEncoding,
            int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "",
            Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebResponse response = CreatePostFileHttpResponse(url, filePath, timeout, userAgent, cookies, Referer,
                headers, contentType);

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Get(string url, Encoding responseEncoding, int? timeout = 300, string userAgent = "", CookieCollection cookies = null
            , string Referer = "", Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebResponse response = CreateGetHttpResponse(url, timeout, userAgent, cookies, Referer, headers, contentType);

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T Get<T>(string url, Encoding responseEncoding, int? timeout = 300, string userAgent = "",
            CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null,
            string contentType = "application/x-www-form-urlencoded")
        {
            return JsonConvert.DeserializeObject<T>(Get(url, responseEncoding, timeout, userAgent, cookies, Referer, headers, contentType));
        }

        public byte[] GetFile(string url, out Dictionary<string, string> header, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null)
        {
            HttpWebResponse response = CreateGetHttpResponse(url, timeout, userAgent, cookies, Referer, headers);

            header = new Dictionary<string, string>();

            foreach (string key in response.Headers.AllKeys)
            {
                header.Add(key, response.Headers[key]);
            }

            try
            {
                System.IO.Stream st = response.GetResponseStream();

                byte[] by = new byte[response.ContentLength];

                st.Read(by, 0, by.Length);

                st.Close();

                return by;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Stream GetStream(string url, int? timeout = 300, string userAgent = "", CookieCollection cookies = null, string Referer = "", Dictionary<string, string> headers = null)
        {
            HttpWebResponse response = CreateGetHttpResponse(url, timeout, userAgent, cookies, Referer, headers);

            return response.GetResponseStream();
        } 
        #endregion

        #region 文件下载
        public static void UpLoadFileByWebClient(string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(filePath))
            {
                return;
            }
            WebClient webClient = new WebClient();
            string fileUrl = System.Web.HttpContext.Current.Server.MapPath(filePath + "/" + fileName);
            try
            {
                WebRequest webRequest = WebRequest.Create(fileUrl);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>alert('" + ex.Message + "');</script>");
            }
            try
            {
                webClient.DownloadFile(fileUrl, fileName);
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>alert('下载成功!');</script>");
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>alert('" + ex.Message + "')</script>");
            }
        }

        public static void UploadFileByWriteFile(string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(fileName))
            {
                return;
            }
            string fileUrl = System.Web.HttpContext.Current.Server.MapPath(filePath + "/" + fileName);
            try
            {
                FileInfo fileInfo = new FileInfo(fileUrl);
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                System.Web.HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                System.Web.HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.Close();
            }
            catch (Exception)
            {

                throw;
            }
        } 
        #endregion
    }
}
