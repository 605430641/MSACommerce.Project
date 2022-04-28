using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileFramework.WebCore.FilterExtend
{
    /// <summary>
    /// 日志记录Filter
    /// </summary>
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private ILogger<LogActionFilterAttribute> _logger = null;
        public LogActionFilterAttribute(ILogger<LogActionFilterAttribute> logger)
        {
            this._logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string url = context.HttpContext.Request.Path.Value;
            string argument = JsonConvert.SerializeObject(context.ActionArguments);
            this._logger.LogInformation($"{url}----->argument={argument}");
            //Console.WriteLine($"This {nameof(CustomGlobalFilterAttribute)} OnActionExecuting{this.Order} ");
        }

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    Console.WriteLine($"This {nameof(CustomGlobalFilterAttribute)} OnActionExecuted{this.Order}");
        //}
        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    Console.WriteLine($"This {nameof(CustomGlobalFilterAttribute)} OnResultExecuting{this.Order}");
        //}
        //public override void OnResultExecuted(ResultExecutedContext context)
        //{
        //    Console.WriteLine($"This {nameof(CustomGlobalFilterAttribute)} OnResultExecuted{this.Order}");
        //}
    }

    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"This {nameof(LogActionFilterAttribute)} OnActionExecuted{this.Order}");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"This {nameof(LogActionFilterAttribute)} OnActionExecuting{this.Order}");
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"This {nameof(LogActionFilterAttribute)} OnResultExecuting{this.Order}");
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"This {nameof(LogActionFilterAttribute)} OnResultExecuted{this.Order}");
        }
    }

    public class CustomControllerFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"This {nameof(CustomControllerFilterAttribute)} OnActionExecuted {this.Order}");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"This {nameof(CustomControllerFilterAttribute)} OnActionExecuting{this.Order}");
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"This {nameof(CustomControllerFilterAttribute)} OnResultExecuting{this.Order}");
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"This {nameof(CustomControllerFilterAttribute)} OnResultExecuted{this.Order}");
        }
    }

}
