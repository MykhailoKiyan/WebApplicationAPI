using Swashbuckle.AspNetCore.Filters;

using WebApplicationAPI.Contracts.V1.Responses;

namespace WebApplicationAPI.SwaggerExamples.V1.Responses {
    public class TagResponseExample : IExamplesProvider<TagResponse> {
        public TagResponse GetExamples() {
            return new TagResponse {
                Name = "new tag"
            };
        }
    }
}
