using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xamarin.Forms;

namespace Epitech.Epicture.Model
{
    internal class ImgurGaleryAsset : ImgurBaseModel
    {
        private WeakReference<ImageSource> _contentImage;

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("datetime")]
        public int Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("animated")]
        public bool IsAnimated { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("is_ad")]
        public bool IsAd { get; set; }

        [JsonProperty("in_gallery")]
        public bool InGallery { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("is_album")]
        public bool IsAlbum { get; set; }

        [JsonProperty("mp4")]
        public string Mp4Source { get; set; }

        public double Ratio => (double)Width / (double)Height;

        public bool ShouldDisplay
        {
            get
            {
                if (IsAlbum)
                    return false;
                return true;
            }
        }


        public Uri ContentImageMedium => new Uri(IsAnimated && !string.IsNullOrEmpty(Mp4Source) ? Mp4Source : $"http://i.imgur.com/{Id}l.{Link.Substring(Link.Length - 3, 3)}");

        public ImageSource ContentImageFull
        {
            get
            {
                ImageSource Create() => new UriImageSource()
                {
                    Uri = new Uri(Link),
                    CachingEnabled = true,
                    CacheValidity = TimeSpan.MaxValue
                };

                if (_contentImage == null)
                    _contentImage = new WeakReference<ImageSource>(Create());
                if (_contentImage.TryGetTarget(out ImageSource res))
                    return res;
                _contentImage.SetTarget(Create());
                return _contentImage.TryGetTarget(out res) ? res : null;
            }
        }
    }
}
