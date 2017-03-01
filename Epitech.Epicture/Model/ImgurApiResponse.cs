using Newtonsoft.Json;

namespace Epitech.Epicture.Model
{
    public class ImgurApiResponse
    {
        [JsonProperty("data")]
        public dynamic Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
