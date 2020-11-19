using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Utils;
using System.Web.Mvc;

namespace AVWeb.Filter
{
    public class RightsAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var uNameStr = filterContext.RequestContext.HttpContext.Request.Cookies.AllKeys.FirstOrDefault(x => x == "uName");
            var tokenStr = filterContext.RequestContext.HttpContext.Request.Cookies.AllKeys.FirstOrDefault(x => x == "token");

            if (uNameStr != null && tokenStr != null)
            {
                var uName = filterContext.RequestContext.HttpContext.Request.Cookies.Get("uName");
                var token = filterContext.RequestContext.HttpContext.Request.Cookies.Get("token");

                var guid = CacheTools.GetCache<string>(uName.Value);

                if (token.Value == guid)
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    filterContext.RequestContext.HttpContext.Response.Redirect("/webav/NoRights");
                }
            }
            else
            {
                filterContext.RequestContext.HttpContext.Response.Redirect("/webav/NoRights");
            }         
        }
    }
}