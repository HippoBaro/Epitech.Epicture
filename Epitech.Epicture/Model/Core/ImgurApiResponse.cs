using System.Collections.Generic;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Core
{
    public class ImgurApiResponse<TInnerType>
    {
        [JsonProperty("data")]
        public TInnerType Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
