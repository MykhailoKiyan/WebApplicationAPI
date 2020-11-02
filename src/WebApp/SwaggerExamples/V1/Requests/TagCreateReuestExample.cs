using Swashbuckle.AspNetCore.Filters;

using WebApplicationAPI.Contracts.V1.Requests;

namespace WebApplicationAPI.SwaggerExamples.Requests {
    public class TagCreateReuestExample : IExamplesProvider<TagCreateRequest> {
        public TagCreateRequest GetExamples() {
            return new TagCreateRequest {
                TagName = "new tag"
            };
        }
    }
}
