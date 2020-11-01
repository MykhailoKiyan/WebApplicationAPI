using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Domain {
    public class Post {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }

        public virtual ICollection<PostTag>? Tags { get; set; }
    }
}
