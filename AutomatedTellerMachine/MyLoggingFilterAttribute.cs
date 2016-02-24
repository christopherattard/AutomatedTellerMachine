using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomatedTellerMachine
{
    public class MyLoggingFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            Console.WriteLine("User Address is: {0}", request.UserHostAddress);
            base.OnActionExecuted(filterContext);
        }
    }
}