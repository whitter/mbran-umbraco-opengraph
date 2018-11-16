using System.Collections.Generic;
using Newtonsoft.Json;

namespace MBran.OpenGraph.Models
{
    public class OpenGraph
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("image")] public int? ImageId { get; set; }

        [JsonProperty("metadata")] public IEnumerable<OpenGraphMetaData> Metadata { get; set; }
    }
}