using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Domain;
using WebApplicationAPI.IntegrationTests.ExtensionMethods;
using Xunit;

namespace WebApplicationAPI.IntegrationTests.PostsControllerTests {
  public class GetAllTests : BaseTest {
    [Fact]
    public async Task GetAll_WithoutAnyPosts_ShouldReturnEmptyResponse()
    {
      // Arrenge
      await this.AuthenticateAsync();

      // Act
      var response = await this.Client.GetAsync(ApiRoutes.Posts.GetAll);

      // Assert
      response.StatusCode.Should().Be(HttpStatusCode.OK);
      (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
    }
  }
}
