using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebApplicationAPI.ExtensionMethods {
    public static class GeneralExtensions {
        public static Guid? GetUserId(this HttpContext httpContext) {
            if (httpContext.User == null) return null;
            if (Guid.TryParse(httpContext.User.Claims.Single(x => x.Type == "id").Value, out Guid result)) {
                return result;
            } else {
                return null;
            }
        }
    }
}
