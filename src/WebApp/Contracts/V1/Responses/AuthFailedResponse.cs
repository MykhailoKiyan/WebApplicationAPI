using System.Collections.Generic;
using System.Linq;

namespace WebApplicationAPI.Contracts.V1.Responses {
    public class AuthFailedResponse {
        public IEnumerable<string> Errors { get; set; }
    }
}
