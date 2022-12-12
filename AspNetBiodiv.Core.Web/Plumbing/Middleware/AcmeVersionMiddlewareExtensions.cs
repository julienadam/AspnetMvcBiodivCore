namespace AspNetBiodiv.Core.Web.Plumbing.Middleware;

public static class AcmeVersionMiddlewareExtensions
{
    public static IApplicationBuilder UseAcmeVersion(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AcmeVersionMiddleware>();
    }
}