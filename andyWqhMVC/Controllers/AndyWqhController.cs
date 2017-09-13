using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace andyWqhMVC.Controllers
{
    public class AndyWqhController : Controller
    {
        //ASP.NET MVC的Controller接收输入
        //1.从Controller的上下文获取
        //2.从Action参数中获取
        //3.利用ASP.NET MVC的model 绑定特性
        // GET: /andyWqh/
        public ActionResult Index()
        {
            // 通过上下文对象获取变量的值
            ViewBag.Message = "This is my study page !";
            ViewBag.UserName = string.IsNullOrEmpty(HttpContext.User.Identity.Name) ? "andyWqh" : User.Identity.Name;
            ViewBag.ServerName = string.IsNullOrEmpty(HttpContext.Server.MachineName) ? "andyWqh" : Server.MachineName;
            ViewBag.ClientIp = string.IsNullOrEmpty(HttpContext.Request.UserHostAddress) ? "192.168.0.136" : Request.UserHostAddress;
            ViewBag.DateStamp = HttpContext.Timestamp;
            ViewBag.ClientName = string.IsNullOrEmpty(HttpContext.Request.UserHostName) ? "andyWqh" : Request.UserHostName;

            //通过上下文请求对象获取表单提交的参数值
            ViewBag.oldProductName = HttpContext.Request.Form["oldName"];
            ViewBag.newProductName = HttpContext.Request.Form["newName"];

            return View(HttpContext.Timestamp);
        }

        public ActionResult MyView()
        {
            ViewBag.Description = "Hello ! My name is andyWqh and I am come  from GanZhan JiangXi ";
            ViewBag.UserName =  HttpContext.User.Identity.Name;
            ViewBag.ServerName =  Server.MachineName;
            ViewBag.ClientIp = Request.UserHostAddress;
            ViewBag.DateStamp = HttpContext.Timestamp;
            ViewBag.ClientName =  Request.UserHostName;
            return View("Index",DateTime.Now);
        }

        public ActionResult RedirectUrl()
        {
            ViewBag.Description = "Hello ! My name is andyWqh and I am come  from GanZhan JiangXi ";
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.ServerName = Server.MachineName;
            ViewBag.ClientIp = Request.UserHostAddress;
            ViewBag.DateStamp = HttpContext.Timestamp;
            ViewBag.ClientName = Request.UserHostName;
            return   RedirectToAction("Index",DateTime.Now); 
        }
    }
}