using System.Collections.Generic;

namespace WebApplicationAPI.Contracts.V1.Responses {
    public class ErrorResponse {
        public IList<ErrorModel> Errors { get; } = new List<ErrorModel>();
    }
}
