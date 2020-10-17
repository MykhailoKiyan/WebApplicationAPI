using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using WebApplicationAPI.Domain.Identity;

namespace WebApplicationAPI.Domain {
    public class Tag {
        [Key]
        public string Name { get; set; }

        public string? CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public User? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
