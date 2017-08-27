/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 *机器名称：ANDYWQH
 *公司名称：
 *命名空间：andyWqhMVC
 *文件名：  UserAgentConstraint
 *版本号：  V1.0.0.0
 *唯一标识：d17f3dcd-2312-440c-8092-9212a8c2d759
 *当前的用户域：andyWqh
 *创建人：  andysunWqh
 *电子邮箱：andyWqh@163.com
 *创建时间：2017/8/27 17:10:18
 *描述：使用接口IRouteConstraint自定义一个asp.net mvc路由约束
 *
 *=====================================================================
 *修改标记
 *修改时间：2017/8/27 17:10:18
 *修改人： andysunWqh
 *版本号： V1.0.0.0
 *描述：使用接口IRouteConstraint自定义一个asp.net mvc路由约束
 *
/***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace andyWqhMVC
{
    /// <summary>
    /// 使用接口IRouteConstraint自定义一个asp.net mvc路由约束
    /// </summary>
    public class UserAgentConstraint:IRouteConstraint
    {
        private string _requireUserAgent;

        public UserAgentConstraint(string agentParam)
        {
            _requireUserAgent = agentParam;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
            RouteValueDictionary values, RouteDirection routeDirection)
        {

            return httpContext.Request.UserAgent != null &&
                   httpContext.Request.UserAgent.Contains(_requireUserAgent);
        }
    }
}