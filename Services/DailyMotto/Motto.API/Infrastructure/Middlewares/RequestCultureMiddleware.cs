using System.Globalization;

namespace Motto.API.Infrastructure.Middlewares
{
    /// <summary>
    /// 自定义中间件
    /// 该中间件通过查询字符串设置当前请求的区域性
    /// </summary>
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(Convert.ToString(cultureQuery));

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }

    /// <summary>
    /// 通常，创建扩展方法以通过 IApplicationBuilder 公开中间件
    /// </summary>
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
