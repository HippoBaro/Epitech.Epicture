using Epitech.Epicture.Model.Core.Contract;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Core
{
    internal class ImgurBaseModel : IAsset
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
