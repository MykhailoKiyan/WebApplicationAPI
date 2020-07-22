using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
// using WebApplicationAPI.IntegrationTests.ExtensionMethods;
using Xunit;

namespace WebApplicationAPI.IntegrationTests.PostsControllerTests {
  public class GetTests : BaseTest {
    [Fact]
    public async Task Get_ReturnPost_WhenPostExists()
    {
      // Arrenge
      await this.AuthenticateAsync();
      var createdPost = await this.CreatePostAsync(new PostCreateRequest { Name = "TestPost" });

      // Act
      var response = await this.Client.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Id.ToString()));

      // Assert
      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var returnedPost = await response.Content.ReadAsAsync<Domain.Post>();
      returnedPost.Id.Should().Be(createdPost.Id);
      returnedPost.Name.Should().Be("TestPost");
    }
  }
}
