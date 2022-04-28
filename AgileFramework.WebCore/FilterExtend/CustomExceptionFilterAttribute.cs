using AgileFramework.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace AgileFramework.WebCore.FilterExtend
{
    public class CustomExceptionFilterAttribute : IExceptionFilter
    {
        private ILogger<CustomExceptionFilterAttribute> _logger = null;
        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            this._logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                context.Result = new JsonResult(new AjaxResult()
                {
                    Message = "操作失败",
                    OtherValue = context.Exception.Message,
                    Result = false
                });
                this._logger.LogError(context.Exception.Message);
            }
            context.ExceptionHandled = true;
        }
    }
}
