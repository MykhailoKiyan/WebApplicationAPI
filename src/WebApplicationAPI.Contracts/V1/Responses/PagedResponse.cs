using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationAPI.Contracts.V1.Responses {
    public class PagedResponse<T> {
        public IEnumerable<T> Data { get; private set; }

        public int? PageNumber { get; private set; }

        public int? PageSize { get; private set; }

        public string? PreviosPage { get; private set; }

        public string? NextPage { get; private set; }

        public PagedResponse() { }

        public PagedResponse(
                IEnumerable<T> data,
                int? pageNumber = null,
                int? pageSize = null,
                string? nextPage = null,
                string? previosPage = null) {

            this.Data = data;
            this.NextPage = nextPage;
            this.PreviosPage = previosPage;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
