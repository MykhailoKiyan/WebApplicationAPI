using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApplicationAPI.IntegrationTests.Extensions;
using WebApplicationAPI.Contracts.V1.Responses;

namespace WebApplicationAPI.IntegrationTests.PostsControllerTests {
    public class GetAllTests : BaseTest {
        public GetAllTests(WebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task GetAll_WithoutAnyPosts_ShouldReturnEmptyResponse()
        {
            // Arrenge
            var client = ArrangeHttpClient();
            await client.AuthenticateAsync();

            // Act
            var (response, result) = await client.ExecuteRequest<PagedResponse<PostResponse>>(HttpMethod.Get, ApiRoutes.Posts.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var posts = await response.Content.ReadAsAsync<PagedResponse<PostResponse>>();
            posts.Data.Should().BeEmpty();
        }
    }
}
