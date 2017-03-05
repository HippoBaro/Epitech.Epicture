using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Core
{
    internal class ImgurApiResponse<TInnerType>
    {
        [JsonProperty("data")]
        public TInnerType Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
