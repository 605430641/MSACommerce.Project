using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AgileFramework.WebCore.MiddlewareExtend
{
    /// <summary>
    /// 处理Option预请求的中间件
    /// </summary>
    public class PreOptionRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public PreOptionRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 解决浏览器 Axios跨域请求是 浏览器会先发起Options与请求处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                return;
            }
            await _next.Invoke(context);
        }
    }

    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class PreOptionsRequestMiddlewareExtensions
    {
        /// <summary>
        /// 处理Option预请求的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePreOptionsRequest(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PreOptionRequestMiddleware>();
        }
    }

}
