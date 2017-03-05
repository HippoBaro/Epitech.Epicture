using System.Collections.Generic;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Imgur.Core;
using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Imgur
{
    internal class ImgurComment : ImgurBaseModel, IAssetComment
    {
        [JsonProperty("image_id")]
        public string ImageId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("author_id")]
        public int AuthorId { get; set; }

        [JsonProperty("on_album")]
        public bool OnAlbum { get; set; }

        [JsonProperty("album_cover")]
        public object AlbumCover { get; set; }

        [JsonProperty("ups")]
        public int Ups { get; set; }

        [JsonProperty("downs")]
        public int Downs { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("datetime")]
        public int Timestamp { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("vote")]
        public object Vote { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("children")]
        public List<ImgurComment> Childrens { get; set; }
    }
}
