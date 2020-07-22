using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Data;
using Xunit;
// using WebApplicationAPI.IntegrationTests.ExtensionMethods;
using WebApplicationAPI.Contracts.V1.Responses;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;

namespace WebApplicationAPI.IntegrationTests {
  [Trait("Category", "Integration")]
  public abstract class BaseTest {
    private readonly HttpClient client;
    private readonly InMemoryDatabaseRoot databaseRoot = new InMemoryDatabaseRoot();

    protected HttpClient Client => this.client;

    public BaseTest() {
      var app = new WebApplicationFactory<Startup>()
        .WithWebHostBuilder(builder => {
          builder.ConfigureServices(services => {
            // Remove the app's Db Context registration.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Add Db Context using an in-memory database for testing.
            services.AddDbContext<DataContext>(options => {
              options.UseInMemoryDatabase("InMemoryDbForTesting", this.databaseRoot);
            });
          });
        });
      this.client = app.CreateClient();
    }

    protected async Task AuthenticateAsync() {
      this.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await this.GetJwtAsync());
    }

    protected async Task<PostResponse> CreatePostAsync(PostCreateRequest post) {
      var response = await this.Client.PostAsJsonAsync(ApiRoutes.Posts.Create, post);
      return await response.Content.ReadAsAsync<PostResponse>();
    }

    private async Task<string> GetJwtAsync() {
      var response = await this.client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest {
        Email = "test@gmail.com",
        Password = "SomePa$$word1234"
      });
      var registration = await response.Content.ReadAsAsync<AuthSuccessResponse>();
      return registration.Token;
    }
  }
}
