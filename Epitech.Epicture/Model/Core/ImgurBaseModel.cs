using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Core
{
    internal class ImgurBaseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
