using System;
using System.Collections.Generic;

namespace WebApplicationAPI.Contracts.V1.Responses {
    public class PostResponse {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<TagResponse> Tags { get; set; }
    }
}
