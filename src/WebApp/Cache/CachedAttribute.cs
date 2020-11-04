using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using WebApplicationAPI.Services;

namespace WebApplicationAPI.Cache {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter {
        readonly int timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds) {
            this.timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            // before
            // check if request is cached
            // if true return
            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            if (!cacheSettings.Enabled) {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCachService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse)) {
                context.Result = new ContentResult {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            var executedContext = await next();

            // after
            // get the value
            // cache the response
            if (executedContext.Result is OkObjectResult ok)
                await cacheService.CacheResponseAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request) {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key)) keyBuilder.Append($"|{key}-{value}");

            return keyBuilder.ToString();
        }
    }
}
