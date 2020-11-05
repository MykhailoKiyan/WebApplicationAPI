namespace WebApplicationAPI.Contracts.V1.Requests.Queries {
    public class PaginationQuery {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public PaginationQuery() {
            this.PageNumber = 1;
            this.PageSize = 100;
        }

        public PaginationQuery(int pageNumber, int pageSize) {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
        }
    }
}
