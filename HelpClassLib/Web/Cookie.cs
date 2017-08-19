using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Web;

namespace HelpClassLib.Web
{
    /// <summary>
    /// Class Cookie. This class cannot be inherited.
    /// </summary>
    public sealed class Cookie
    {
        static readonly string SessionKey = ConfigurationManager.AppSettings["SessionKey"];
        static readonly string CookieDomain = ConfigurationManager.AppSettings["CookieDomain"];

        /// <summary>
        /// Gets the specified cookie name value.
        /// </summary>
        /// <param name="name">The cookie name.</param>
        /// <returns>System.String.</returns>
        public static string Get(string name)
        {
            //获取客户端请求服务器指定名称的cookie对象
            var cookie = HttpContext.Current.Request.Cookies.Get(name);
            if (cookie == null)
            {
                return string.Empty;
            }
            return cookie.Value;
        }

        /// <summary>
        /// Gets the session identifier.key
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetSessionId()
        {
            return Get(SessionKey);
        }

        /// <summary>
        /// Removes the specified name cookie.
        /// </summary>
        /// <param name="name">The cookie name.</param>
        public static void Remove(string name)
        {
            //创建指定名称的cookie对象，通过设置过期时间在当前上下文cookie集合中删除
            var cookie = new HttpCookie(name)
            {
                Value = string.Empty,
                Expires = DateTime.Now.AddYears(-100),
                HttpOnly = false
            };
            HttpContext.Current.Request.Cookies.Add(cookie);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Clears HttpContext  All Cookies List.
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Response.Cookies.Clear();
        }

        /// <summary>
        /// Sets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="httpOnly">if set to <c>true</c> [HTTP only].</param>
        public static void Set(string name, string value, bool httpOnly = false)
        {
            var cookie = new HttpCookie(name) 
            { 
                Value  =  value,
                HttpOnly = httpOnly,

            };
            if (!string.IsNullOrEmpty(CookieDomain))
            {
                cookie.Domain = CookieDomain;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Sets the session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public static void SetSessionId(string sessionId)
        {
            Set(SessionKey, sessionId);
        }
    }
}
