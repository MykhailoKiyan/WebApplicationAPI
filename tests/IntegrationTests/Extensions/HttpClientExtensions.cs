using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using WebApplicationAPI.Contracts.V1;
using WebApplicationAPI.Contracts.V1.Requests;
using WebApplicationAPI.Contracts.V1.Responses;

namespace WebApplicationAPI.IntegrationTests.Extensions {
    public static class HttpClientExtensions {
        public static async Task<(HttpResponseMessage Response, T Result)> ExecuteRequest<T>(
                this HttpClient client,
                HttpMethod type,
                string url,
                object body = null,
                Dictionary<string, string> headers = null) {

            var request = new HttpRequestMessage(type, url);
            if (body != null) {
                var dataAsString = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(dataAsString);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            if (headers != null) {
                foreach (var header in headers) {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(request);
            var result = await response.ReadResponseAs<T>();
            return (response, result);
        }

        public static async Task<T> ReadResponseAs<T>(this HttpResponseMessage response) {
            var payload = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<T>(payload);
            return responseContent;
        }

        public static async Task AuthenticateAsync(this HttpClient client) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(client));
        }

        private static async Task<string> GetJwtAsync(HttpClient client) {
            var response = await client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest {
                Email = "test@gmail.com",
                Password = "SomePa$$word1234"
            });
            var registration = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registration.Token;
        }
    }
}
