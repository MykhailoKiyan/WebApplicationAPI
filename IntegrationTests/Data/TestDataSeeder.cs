using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using WebApplicationAPI.Data;
using WebApplicationAPI.Domain;

namespace WebApplicationAPI.IntegrationTests.Data {
    public class TestDataSeeder {
        private readonly DataContext context;

        public TestDataSeeder(DataContext context) {
            this.context = context;
        }

        public void Seed() {
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
            // Seeds.ForEach(x => this.context.Posts.Add(x));
            // this.context.SaveChanges();
        }

        public static List<Post> Seeds {
            get => Enumerable.Range(1, 300).Select(x => new Post { Name = $"Post #{x}" }).ToList();
        }
    }
}
