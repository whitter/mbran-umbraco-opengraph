using Newtonsoft.Json;

namespace MBran.OpenGraph.Models
{
    public class OpenGraphMetaData
    {
        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("value")] public string Value { get; set; }
    }
}