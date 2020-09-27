using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace WebApplicationAPI.Domain {
  [CosmosCollection("posts")]
  public class CosmosPostDto {
    [CosmosPartitionKey]
    [JsonProperty("id")]
    public string? Id { get; set; }
    public string? Name { get; set; }
  }
}
