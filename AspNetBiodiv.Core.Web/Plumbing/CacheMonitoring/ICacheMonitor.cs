namespace AspNetBiodiv.Core.Web.Plumbing.CacheMonitoring;

public interface ICacheMonitor
{
    int CacheMisses { get; }
    int CacheHits { get; }
    void AddCacheHit();
    void AddCacheMiss();
}