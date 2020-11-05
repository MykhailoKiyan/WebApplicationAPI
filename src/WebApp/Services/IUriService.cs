using System;

using WebApplicationAPI.Contracts.V1.Requests.Queries;

namespace WebApplicationAPI.Services {
    public interface IUriService {
        Uri GetPostUri(string postId);

        Uri GetAllPostsUri(PaginationQuery? paginationQuery = null);
    }
}
