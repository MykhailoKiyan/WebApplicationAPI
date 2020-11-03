using System.Threading.Tasks;
using Refit;

using WebApplicationAPI.Contracts.V1.Requests;

using WebApplicationSDK;

namespace WebApplicationSDK.Sample {
    class Program {
        static async Task Main(string[] args) {
            var cachedToket = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");

            var registerResponse = await identityApi.RegisterAsync(new UserRegistrationRequest {
                Email = "sdkaccount@gmail.com",
                Password = "String1!"
            });

            var loginResponse = await identityApi.LoginAsync(new UserLoginRequest {
                Email = "sdkaccount@gmail.com",
                Password = "String1!"
            });

            cachedToket = loginResponse.Content.Token;

            var applicationApi = RestService.For<IApplicationApi>("https://localhost:5001", new RefitSettings {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToket)
            });

            var allPosts = await applicationApi.GetAllAsync();

            var createdPost = await applicationApi.CreateAsync(new PostCreateRequest {
                Name = "Post from SDK",
                Tags = new[] { "sdk1", "sdk2" }
            });

            var retrievedPost = await applicationApi.GetAsync(createdPost.Content.Id);

            var updatedPost = await applicationApi.UpdateAsync(createdPost.Content.Id, new PostUpdateRequest { 
                Name = "The new name from SDK"
            });

            var deletedPost = await applicationApi.DeleteAsync(createdPost.Content.Id);
        }
    }
}
