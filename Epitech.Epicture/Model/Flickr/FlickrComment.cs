using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Flickr.Core;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Flickr
{
    internal class FlickrComment : FlickrBaseModel, IAssetComment
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("author_is_deleted")]
        public int AuthorIsDeleted { get; set; }

        [JsonProperty("authorname")]
        public string Authorname { get; set; }

        [JsonProperty("iconserver")]
        public string Iconserver { get; set; }

        [JsonProperty("iconfarm")]
        public int Iconfarm { get; set; }

        [JsonProperty("datecreate")]
        public string Datecreate { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("path_alias")]
        public string PathAlias { get; set; }

        [JsonProperty("realname")]
        public string Realname { get; set; }

        [JsonProperty("_content")]
        public string Comment { get; set; }
    }
}
