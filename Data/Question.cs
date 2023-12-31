using Newtonsoft.Json;

namespace FakersGame.Data
{
    public class Question
    {
        [JsonProperty(nameof(FakerVersion))]
        public string FakerVersion { get; set; }

        [JsonProperty(nameof(RealVersion))]
        public string RealVersion { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty(nameof(LastAccessedTime))]
        public string LastAccessedTime { get; set; }
    }
}
