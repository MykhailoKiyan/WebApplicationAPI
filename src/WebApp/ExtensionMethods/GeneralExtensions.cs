using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebApplicationAPI.ExtensionMethods {
    public static class GeneralExtensions {
        public static string GetUserId(this HttpContext httpContext) {
            if (httpContext.User == null) {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}
