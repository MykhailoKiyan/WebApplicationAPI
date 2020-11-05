using Microsoft.AspNetCore.WebUtilities;

using System;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests.Queries;

namespace WebApplicationAPI.Services {
    public class UriService : IUriService {
        readonly string baseUri;

        public UriService(string baseUri) {
            this.baseUri = baseUri;
        }

        public Uri GetAllPostsUri(PaginationQuery? paginationQuery = null) {
            var uri = new Uri(this.baseUri);
            if (paginationQuery == null) return uri;

            var modifiedUri = QueryHelpers.AddQueryString(this.baseUri, "pageNumber", paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());
            return new Uri(modifiedUri);
        }

        public Uri GetPostUri(string postId) {
            return new Uri(this.baseUri + ApiRoutes.Posts.Get.Replace("{postId}", postId));
        }
    }
}
