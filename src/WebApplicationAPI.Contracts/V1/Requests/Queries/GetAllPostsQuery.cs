using Microsoft.AspNetCore.Mvc;

using System;

namespace WebApplicationAPI.Contracts.V1.Requests.Queries {
    public class GetAllPostsQuery {
        [FromQuery(Name = "userId")]
        public Guid? UserId { get; set; }
    }
}
