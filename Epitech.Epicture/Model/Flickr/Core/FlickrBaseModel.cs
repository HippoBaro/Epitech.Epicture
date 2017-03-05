using Epitech.Epicture.Model.Contract;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Flickr.Core
{
    internal class FlickrBaseModel : IAsset
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
