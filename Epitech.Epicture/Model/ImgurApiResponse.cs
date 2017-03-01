using System.Collections.Generic;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model
{
    public class ImgurApiResponse<TInnerType>
    {
        [JsonProperty("data")]
        public List<TInnerType> Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
