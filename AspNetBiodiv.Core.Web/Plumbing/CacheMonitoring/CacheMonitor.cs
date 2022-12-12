namespace AspNetBiodiv.Core.Web.Plumbing.CacheMonitoring
{
    public class CacheMonitor : ICacheMonitor
    {
        private int cacheMisses = 0;
        private int cacheHits = 0;

        public int CacheMisses => cacheMisses;

        public int CacheHits => cacheHits;

        public void AddCacheHit()
        {
            Interlocked.Increment(ref cacheHits);
        }

        public void AddCacheMiss()
        {
            Interlocked.Increment(ref cacheMisses);
        }
    }
}
