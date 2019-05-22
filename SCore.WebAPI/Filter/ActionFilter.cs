using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCore.WebAPI.Filter
{
    public class ActionFilter : Attribute, IActionFilter, IExceptionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            log4net.LogicalThreadContext.Properties["response_time"] = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            log4net.LogicalThreadContext.Properties["username"] = context.HttpContext.User.Identity.Name;
            log4net.LogicalThreadContext.Properties["request_uri"] = context.HttpContext.Request.Path;
            log4net.LogicalThreadContext.Properties["headers"] = context.HttpContext.Response.Headers;
            log4net.LogicalThreadContext.Properties["status_code"] = context.HttpContext.Response.StatusCode;
            log4net.LogicalThreadContext.Properties["body"] = "Body";
            Logger.Logger.Log.Info("Message");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            log4net.LogicalThreadContext.Properties["request_time"] = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            log4net.LogicalThreadContext.Properties["username"] = context.HttpContext.User.Identity.Name;
            log4net.LogicalThreadContext.Properties["request_uri"] = context.HttpContext.Request.Path;
            log4net.LogicalThreadContext.Properties["method"] = context.HttpContext.Request.Method.ToString();
            log4net.LogicalThreadContext.Properties["headers"] = context.HttpContext.Request.Headers;
            log4net.LogicalThreadContext.Properties["q_string"] = context.HttpContext.Request.QueryString;
            log4net.LogicalThreadContext.Properties["body"] = "Body";
            Logger.Logger.Log.Info("Message");
        }

        public void OnException(ExceptionContext context)
        {
            log4net.LogicalThreadContext.Properties["trace"] = context.Exception.StackTrace;
            log4net.LogicalThreadContext.Properties["mess"] = context.Exception.Message;
            Logger.Logger.Log.Error("Error");
        }
    }
}
