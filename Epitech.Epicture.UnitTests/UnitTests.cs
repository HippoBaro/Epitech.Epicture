using Epitech.Epicture.Services.Flickr;
using Epitech.Epicture.Services.Imgur;
using Epitech.Epicture.ViewModels;
using NUnit.Framework;

namespace Epitech.Epicture.UnitTests
{
    [TestFixture]
    public class UnitTests
    {
        private GalleryViewModel<ImgurClientService> ImgurGalleryViewModel { get; set; }
        private GalleryViewModel<FlickrClientService> FlickrGalleryViewModel { get; set; }

        [SetUp]
        public void Init()
        {
            ImgurGalleryViewModel = new GalleryViewModel<ImgurClientService>();
            FlickrGalleryViewModel = new GalleryViewModel<FlickrClientService>();
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(true, true);
        }
    }
}
