using System;

using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Domain {
    public class Tag {
        public string Name { get; set; }

        public Guid? CreatorId { get; set; }

        public User? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
