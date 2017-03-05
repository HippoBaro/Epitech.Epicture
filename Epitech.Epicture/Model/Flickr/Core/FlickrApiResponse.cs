using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Flickr.Core
{
    internal class FlickrApiResponse<TInnerType>
    {
        public TInnerType Data { get; set; }

        [JsonProperty("stat")]
        public string Success { get; set; }
    }
}
