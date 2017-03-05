﻿using System;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Imgur.Core;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Epitech.Epicture.Model.Imgur
{
    internal class ImgurGaleryAsset : ImgurBaseModel, IImageAsset
    {
        private WeakReference<ImageSource> _contentImageThumbmail;
        private WeakReference<ImageSource> _contentImageFull;

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

        [JsonProperty("favorite")]
        public bool Favorite { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("is_album")]
        public bool IsAlbum { get; set; }

        public double Ratio => Width / (double)Height;

        public bool ShouldDisplay
        {
            get
            {
                if (IsAlbum)
                    return false;
                return true;
            }
        }


        public ImageSource ContentImageMedium
        {
            get
            {
                ImageSource Create()
                {
                    var link = new Uri($"http://i.imgur.com/{Id}l.{Link.Substring(Link.Length - 3, 3)}");
                    return new UriImageSource
                    {
                    Uri = link,
                    CachingEnabled = false,
                    CacheValidity = TimeSpan.MaxValue
                };}

                if (_contentImageThumbmail == null)
                    _contentImageThumbmail = new WeakReference<ImageSource>(Create());
                if (_contentImageThumbmail.TryGetTarget(out ImageSource res))
                    return res;
                _contentImageThumbmail.SetTarget(Create());
                return _contentImageThumbmail.TryGetTarget(out res) ? res : null;
            }
        }

        public ImageSource ContentImageFull
        {
            get
            {
                ImageSource Create() => new UriImageSource
                {
                    Uri = new Uri(Link),
                    CachingEnabled = false,
                    CacheValidity = TimeSpan.MaxValue,
                };

                if (_contentImageFull == null)
                    _contentImageFull = new WeakReference<ImageSource>(Create());
                if (_contentImageFull.TryGetTarget(out ImageSource res))
                    return res;
                _contentImageFull.SetTarget(Create());
                return _contentImageFull.TryGetTarget(out res) ? res : null;
            }
        }
    }
}