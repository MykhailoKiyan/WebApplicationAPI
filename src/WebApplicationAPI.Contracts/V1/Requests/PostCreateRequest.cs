using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplicationAPI.Contracts.V1.Requests {
    public class PostCreateRequest {
        public string Name { get; set; }

        public IEnumerable<string>? Tags { get; set; }
    }
}
