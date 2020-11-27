using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Utils;
using System.Web.Mvc;
using Model.WebModel;
using DataBaseManager.ScanDataBaseHelper;

namespace AVWeb.Filter
{
    public class BaseAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            WebViewLog log = new WebViewLog();

            log.IPAddress = filterContext.RequestContext.HttpContext.Request.UserHostAddress;
            log.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            log.UserAgent = filterContext.RequestContext.HttpContext.Request.UserAgent;
            log.Action = filterContext.ActionDescriptor.ActionName;

            var arrParameter = filterContext.ActionDescriptor.GetParameters();
            string paramter = "?";
            foreach (var pName in arrParameter)
            {
                var parameterValue = filterContext.ActionParameters[pName.ParameterName];

                paramter += pName.ParameterName + "=" + parameterValue.ToString() + "&";
            }
            log.Parameter = paramter;
          
            ScanDataBaseManager.InserWebViewLog(log);
            base.OnActionExecuting(filterContext);
        }
    }
}