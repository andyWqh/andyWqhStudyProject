using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace andyWqhMVC
{
    public class RouteConfig
    {
        /// <summary>
        /// 项目默认自动生成默认路由规则
        /// 1、规则名：Default
        /// 2、URL分段：{controller}/{action}/{id}，分别有三段，第一段对应controller参数，
        /// 第二段为action参数，第三段为id参数
        /// 3、URL段的默认值：controller为Home，action为Index，
        /// id = UrlParameter.Optional表示该参数为可选的。
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //创建一个简单的自定义路由规则,并设定action默认值
            routes.MapRoute(
                name:"myRoute",
                url:"{controller}/{action}",
                defaults: new { action ="Index"}
                );

            //调用Add方法添加路由
            //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());
            //routes.Add(myRoute);


            //mvc使用静态URL段
            routes.MapRoute("staticRoute","public/{controller}/{action}",
                new { controller = " Home",action="Index"});

            //mvc使用*来定义变长数量的URL片段
            routes.MapRoute("variableRoute", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            //mvc使用命名空间来为路由的Controller类定优先级
            routes.MapRoute("Namespace",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "andyWqhMVC.Controllers" }
            );

            //mvc定义路由规则的约束
            //用正则表达式限制asp.net mvc路由规则
            //表示只匹配contorller名字以H开头的URL
            routes.MapRoute("constraint", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = "^H.*" },
                new[] { "andyWqhMVC.Controllers" });

            //把asp.net mvc路由规则限制到到具体的值
            //controller和action上都定义了约束，约束是同时起作用是，也就是要同时满足。上面表示只匹配contorller名字以H开头的URL，
            //且action变量的值为Index或者为About的URL
            routes.MapRoute("valueRoute", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = "^H.*", action = "^Index$|^About$" },
                new[] { "andyWqhMVC.Controllers" });

            //把asp.net mvc路由规则限制到到提交请求方式（POST、GET）表示只匹配为GET方式的请求
            routes.MapRoute("MethodRoute", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new
                {
                    controller = "^H.*",
                    action = "Index|About",
                    httpMethod = new HttpMethodConstraint("GET")
                },
                new[] { "andyWqhMVC.Controllers" });

            //使用接口IRouteConstraint自定义一个asp.net mvc路由约束
            //表示这个路由规则只匹配用户使用IE浏览器的请求。利用这点我们就可以实现不同浏览器使用不同的Controller，
            //进行不同的处理。虽然这样做的意义不大，但是不排除有时会有这种变态的需求
            routes.MapRoute("ConstraintRoute", "{controller}/{action}/{id}/{*catchall}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new
                {
                    controller = "^H.*",
                    action = "Index|About",
                    httpMethod = new HttpMethodConstraint("GET", "POST"),
                    customConstraint = new UserAgentConstraint("IE")
                },
                new[] { "andyWqhMVC.Controllers" });


            //mvc跳过、绕开路由系统设定 Content目录下的所有文件都绕开mvc的路由系统
            routes.RouteExistingFiles = true;

            routes.MapRoute("DiskFile", "Content1/StaticContent.html",
                new
                {
                    controller = "Account",
                    action = "LogOn",
                },
                new
                {
                    customConstraint = new UserAgentConstraint("IE")
                });
            routes.IgnoreRoute("Content/*{filename}");
            routes.MapRoute("", "{controller}/{action}");  

            //default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
