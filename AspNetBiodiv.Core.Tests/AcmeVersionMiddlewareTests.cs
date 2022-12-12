using AspNetBiodiv.Core.Web.Plumbing.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AspNetBiodiv.Core.Tests;

public class AcmeVersionMiddlewareTests
{
    [Fact]
    public async Task Middleware_inserts_acme_version_header()
    {
        var middleware = new AcmeVersionMiddleware();
        HttpContext context = new DefaultHttpContext();
        await middleware.InvokeAsync(context, httpContext => Task.CompletedTask);

        Assert.True(context.Response.Headers.TryGetValue("Acme-Version", out var version));
        var expected = typeof(AcmeVersionMiddleware).Assembly.GetName().Version?.ToString();
        Assert.Equal(expected, version);
    }
}