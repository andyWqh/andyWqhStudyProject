using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace andyWqhMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
		
        /// <summary>
        /// Application_Start方法会在网站启动的自动调用
        /// RouteConfig.RegisterRoutes(RouteTable.Routes);这个就是向ASP.NET MVC 框架注册我们自定义的路由规则，让之后的URL能够对应到具体的Action
        /// </summary>
        protected void Application_Start()
        {
            //注册ASP.NET MVC 应用程序的所有区域
            AreaRegistration.RegisterAllAreas();
            //注册全局的Filters过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //注册全局的路由规则
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //注册打包绑定(js，css等)
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
		
		
        // public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        //    routes.MapRoute(
        //        "Default",
        //        "{controller}/{action}/{id}",
        //        new { controller = "Home", action = "Index", id = "" }
        //      );         
        //}
    }
}
