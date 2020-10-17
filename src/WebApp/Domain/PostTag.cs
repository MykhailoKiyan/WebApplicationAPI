using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationAPI.Domain {
    public class PostTag {
        public virtual Tag Tag { get; set; }

        public string TagName { get; set; }

        public virtual Post Post { get; set; }

        public Guid PostId { get; set; }
    }
}
