using Newtonsoft.Json;

namespace Epitech.Epicture.Model.Flickr
{
    internal class FlickrAuthorisation
    {
        public class Content
        {
            [JsonProperty("_content")]
            public string Value { get; set; }
        }

        public class User
        {
            [JsonProperty("nsid")]
            public string Nsid { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("fullname")]
            public string Fullname { get; set; }
        }

        [JsonProperty("token")]
        public Content Token { get; set; }

        [JsonProperty("perms")]
        public Content Perms { get; set; }

        [JsonProperty("user")]
        public User AuthenticatedUser { get; set; }
    }
}
