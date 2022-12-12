using System.Reflection;

namespace AspNetBiodiv.Core.Web.Plumbing.Middleware
{
    public class AcmeVersionMiddleware : IMiddleware
    {
        private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("Acme-Version", Version);
            await next(context);
        }
    }
}
