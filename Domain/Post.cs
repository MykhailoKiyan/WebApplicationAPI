using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationAPI.Domain {
    public class Post {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get;  set; }
    }
}
