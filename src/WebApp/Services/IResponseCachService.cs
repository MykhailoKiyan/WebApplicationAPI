using System;
using System.Threading.Tasks;

namespace WebApplicationAPI.Services {
    public interface IResponseCachService {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);

        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
