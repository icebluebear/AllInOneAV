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
    public class RightsAttribute : System.Web.Mvc.ActionFilterAttribute
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

            var uNameStr = filterContext.RequestContext.HttpContext.Request.Cookies.AllKeys.FirstOrDefault(x => x == "uName");
            var tokenStr = filterContext.RequestContext.HttpContext.Request.Cookies.AllKeys.FirstOrDefault(x => x == "token");

            if (uNameStr != null && tokenStr != null)
            {
                var uName = filterContext.RequestContext.HttpContext.Request.Cookies.Get("uName");
                var token = filterContext.RequestContext.HttpContext.Request.Cookies.Get("token");

                var guid = CacheTools.GetCache<string>(uName.Value);

                if (token.Value == guid)
                {
                    log.IsLogin = 1;
                    ScanDataBaseManager.InserWebViewLog(log);
                    base.OnActionExecuting(filterContext);                   
                }
                else
                {
                    ScanDataBaseManager.InserWebViewLog(log);
                    filterContext.RequestContext.HttpContext.Response.Redirect("/webav/NoRights");                
                }
            }
            else
            {
                ScanDataBaseManager.InserWebViewLog(log);
                filterContext.RequestContext.HttpContext.Response.Redirect("/webav/NoRights");
            }

            ScanDataBaseManager.InserWebViewLog(log);
        }
    }
}