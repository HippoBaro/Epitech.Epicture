using Epitech.Epicture.Model.Contract;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Imgur.Core
{
    public class ImgurBaseModel : IAsset
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
