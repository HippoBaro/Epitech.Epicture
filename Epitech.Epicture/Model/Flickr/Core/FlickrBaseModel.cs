using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
