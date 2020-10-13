using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.TestHost;
using Xunit;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Data;
using WebApplicationAPI.Contracts.V1.Responses;
using WebApplicationAPI.IntegrationTests.Data;
using WebApplicationAPI.IntegrationTests.Extensions;

namespace WebApplicationAPI.IntegrationTests {
    [Trait("Category", "Integration")]
    public abstract class BaseTest : IClassFixture<WebApplicationFactory<Startup>> {

        protected readonly WebApplicationFactory<Startup> appFactory;
        private HttpClient client;
        protected IServiceCollection services;
        private readonly InMemoryDatabaseRoot databaseRoot = new InMemoryDatabaseRoot();

        public BaseTest(WebApplicationFactory<Startup> factory) {
            this.appFactory = factory.WithWebHostBuilder(c => {
                c.ConfigureTestServices(SetupMockServices);
            });
            this.client = this.appFactory.CreateClient();
        }

        public HttpClient ArrangeHttpClient() {
            return this.appFactory.CreateClient();
        }

        protected virtual void SetupMockServices(IServiceCollection services) {
            this.services = services;
            SetupDatabase(services);
        }

        private void SetupDatabase(IServiceCollection services) {
            // Remove the app's Db Context registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<DataContext>));

            if (descriptor != null) {
                services.Remove(descriptor);
            }

            // Add Db Context using an in-memory database for testing.
            services.AddDbContext<DataContext>(options => {
                options.UseInMemoryDatabase("InMemoryDbForTesting", databaseRoot);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            new TestDataSeeder(db).Seed();
        }

        protected async Task<PostResponse> CreatePostAsync(HttpClient client, PostCreateRequest post) {
            var (response, _) = await client.ExecuteRequest<PostCreateRequest>(HttpMethod.Post, ApiRoutes.Posts.Create, post);
            return await response.Content.ReadAsAsync<PostResponse>();

        }
    }
}
