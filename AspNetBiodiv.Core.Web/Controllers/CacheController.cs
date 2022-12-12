using AspNetBiodiv.Core.Web.Plumbing.CacheMonitoring;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace AspNetBiodiv.Core.Web.Controllers
{
    public class CacheController : Controller
    {
        private readonly ICacheMonitor monitor;

        public CacheController(ICacheMonitor monitor)
        {
            this.monitor = monitor;
        }

        public IActionResult Index()
        {
            var ratio = (double) monitor.CacheHits / ((double)monitor.CacheHits + (double)monitor.CacheMisses) * 100d;
            return Content($"Hits: {monitor.CacheHits}\nMisses: {monitor.CacheMisses}\nRatio: {ratio}%");
        }
    }
}
