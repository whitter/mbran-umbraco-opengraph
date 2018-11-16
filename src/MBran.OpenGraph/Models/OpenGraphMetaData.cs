using Newtonsoft.Json;

namespace MBran.OpenGraph.Models
{
    public class OpenGraphMetaData
    {
        [JsonProperty("metadata")] public string Metadata { get; set; }

        [JsonProperty("value")] public string Value { get; set; }
    }
}