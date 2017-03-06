using System;
using System.Collections.Generic;
using System.Linq;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Flickr.Core;
using Newtonsoft.Json;
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

        [JsonProperty("url_l")]
        public string LinkThumbmail { get; set; }

        [JsonProperty("height_l")]
        public int Height { get; set; }

        [JsonProperty("width_l")]
        public int Width { get; set; }

        public bool ShouldDisplay => !string.IsNullOrEmpty(LinkThumbmail);

        [JsonProperty("isfavorite")]
        public bool Favorite { get; set; }

        public double Ratio => Width / (double) Height;

        public ImageSource ContentImageMedium
        {
            get
            {
                if (_contentImageThumbmail == null)
                    _contentImageThumbmail = new WeakReference<ImageSource>(CreateMedium());
				ImageSource res;
                if (_contentImageThumbmail.TryGetTarget(out res))
                    return res;
                _contentImageThumbmail.SetTarget(CreateMedium());
                return _contentImageThumbmail.TryGetTarget(out res) ? res : null;
            }
        }

        public ImageSource ContentImageFull
        {
            get
            {
                if (_contentImageFull == null)
                    _contentImageFull = new WeakReference<ImageSource>(CreateFull());
				ImageSource res;
                if (_contentImageFull.TryGetTarget(out res))
                    return res;
                _contentImageFull.SetTarget(CreateFull());
                return _contentImageFull.TryGetTarget(out res) ? res : null;
            }
        }

		private ImageSource CreateMedium()
		{
			var link = new Uri(LinkThumbmail);
			return new UriImageSource
			{
				Uri = link,
				CachingEnabled = false,
				CacheValidity = TimeSpan.MaxValue
			};
		}

		ImageSource CreateFull() => new UriImageSource
		{
			Uri = new Uri(Sources.Last().Source),
			CachingEnabled = false,
			CacheValidity = TimeSpan.MaxValue,
		};
    }

    public class PersonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => reader.ValueType == typeof(string) ? reader.Value : "";

        public override bool CanConvert(Type objectType) => true;
    }
}
