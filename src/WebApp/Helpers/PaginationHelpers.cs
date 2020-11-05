using System.Collections.Generic;
using System.Linq;

using WebApplicationAPI.Contracts.V1.Requests.Queries;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.Domain;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Helpers {
    public class PaginationHelpers {
        internal static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter paginationFilter, List<T> response) {
            var nextPage = paginationFilter.PageNumber >= 1
                ? uriService.GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
                : null;
            var previousPage = paginationFilter.PageNumber - 1 >= 1
                ? uriService.GetAllPostsUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
                : null;
            var pageNumber = paginationFilter.PageNumber >= 1
                ? paginationFilter.PageNumber
                : (int?)null;
            var pageSize = paginationFilter.PageSize >= 1
                ? paginationFilter.PageSize
                : (int?)null;
            return new PagedResponse<T>(response, pageNumber, pageSize, response.Any() ? nextPage : null, previousPage);
        }
    }
}
