using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HelpClassLib.Web
{
    /// <summary>
    /// Class WebSite. This class cannot be inherited.
    /// </summary>
    public sealed class WebSite
    {
        /// <summary>
        /// Gets the site root URL.
        /// </summary>
        /// <value>The URL.</value>
        public static string Url
        {
            get
            {
                var siteUrl = "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;
                if (!siteUrl.EndsWith("/"))
                {
                    siteUrl = siteUrl + "/";
                }
                return siteUrl;
            }
        }

        /// <summary>
        /// Gets the site appliction virual path.
        /// </summary>
        /// <value>The appliction path.</value>
        public static string ApplictionPath
        {
            get
            {
                if (HttpContext.Current.Request.ApplicationPath.Equals("/"))
                {
                    return string.Empty;
                }
                else
                {
                    return HttpContext.Current.Request.ApplicationPath;
                }
            }
        }

        /// <summary>
        /// Gets the site application base directory.
        /// </summary>
        /// <value>The base directory.</value>
        public static string BaseDirectory
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// Gets the URL without HTTP.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetUrlWithoutHttp()
        {
            var url = GetUrl();
            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                url = url.Substring(7, url.Length - 7);
            }
            if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = url.Substring(8, url.Length - 8);
            }

            return url;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetUrl()
        {
            var strUrl = HttpContext.Current.Request.Url.Authority.Replace(HttpContext.Current.Request.Url.PathAndQuery,"")+ HttpContext.Current.Request.ApplicationPath;
            if (strUrl.EndsWith("/"))
            {
                strUrl = strUrl.Substring(0,strUrl.Length-1);
            }
            return strUrl;
        }

        public static bool IsSearchUrl()
        {
            string Excp = HttpContext.Current.Request.ServerVariables.Get("Http_User_Agent");
            if (Excp.Contains("Baiduspider") || Excp.Contains("Googlebot") || Excp.Contains("Sosospider") || Excp.Contains("Sogou+web+spider"))
            {
                return true;
            }
            return false;
        }
    }
}
