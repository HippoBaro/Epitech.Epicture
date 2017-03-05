using System;
using System.Collections.Generic;
using System.Linq;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Flickr.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Epitech.Epicture.Model.Flickr
{
    internal class FlickrGaleryAsset : FlickrBaseModel, IImageAsset
    {
        private WeakReference<ImageSource> _contentImageThumbmail;
        private WeakReference<ImageSource> _contentImageFull;

        public List<FlickrAssetSource> Sources { get; set; }

        [JsonProperty("title"), JsonConverter(typeof(PersonConverter))]
        public string Title { get; set; }

        public bool ShouldDisplay => Sources != null;

        public bool Favorite => false;

        public double Ratio => (double) Sources.Last().Width / (double) Sources.Last().Height;

        public ImageSource ContentImageMedium
        {
            get
            {
                ImageSource Create()
                {
                    var sizes = Sources.Where(source => source.Label.Split(' ').Length == 1);
                    var link = new Uri(sizes.ElementAt(sizes.Count() / 2).Source);
                    return new UriImageSource
                    {
                        Uri = link,
                        CachingEnabled = false,
                        CacheValidity = TimeSpan.MaxValue
                    };
                }

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
                    Uri = new Uri(Sources.Last().Source),
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

    public class PersonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
                return reader.Value;
            else
            {
                return "";
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
