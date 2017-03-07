using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Epitech.Epicture.Model.Contract;
using Epitech.Epicture.Model.Imgur;
using Epitech.Epicture.Model.Imgur.Core;
using Epitech.Epicture.Services.Contracts;
using Epitech.Epicture.Services.Flickr;
using Epitech.Epicture.Services.Flickr.Core;
using Epitech.Epicture.Services.Imgur;
using Epitech.Epicture.Services.Imgur.Core;
using NUnit.Framework;

namespace Epitech.Epicture.UnitTests
{
    [TestFixture]
    public class FlickrTests
    {
        private IImageClientService ClientService { get; set; }

        [SetUp]
        public void Init()
        {
            ClientService = new FlickrClientService();
        }

        [Test]
        public void ServiceCovarianceAndContravariance()
        {
            var serviceBase = ClientService as IBaseClient;
            var serviceConcreteBase = ClientService as FlickrBaseClient;
            var serviceConcrete = ClientService as FlickrClientService;

            Assert.NotNull(serviceBase);
            Assert.NotNull(serviceConcreteBase);
            Assert.NotNull(serviceConcrete);
        }

        [Test]
        public void AssetCovarianceAndContravariance()
        {
            var image = new ImgurGaleryAsset();

            var assetBase = image as IAsset;
            var assetImageBase = image as IImageAsset;
            var assetConcreteBase = image as ImgurBaseModel;

            Assert.NotNull(assetBase);
            Assert.NotNull(assetImageBase);
            Assert.NotNull(assetConcreteBase);

            var comment = new ImgurComment();

            var commentBase = comment as IAsset;
            var commentImageBase = comment as IAssetComment;
            var commentConcreteBase = comment as ImgurBaseModel;

            Assert.NotNull(commentBase);
            Assert.NotNull(commentImageBase);
            Assert.NotNull(commentConcreteBase);
        }

        [Test]
        public async Task MainGelleryDownload()
        {
            IEnumerable<IImageAsset> assets = await ClientService.GetMainGalery(0);

            Assert.NotNull(assets);
            Assert.IsNotEmpty(assets);
        }

        [Test]
        public async Task ImageMainGalleryPaging()
        {
            IEnumerable<IImageAsset> assetsPage0 = await ClientService.GetMainGalery(0);

            Assert.NotNull(assetsPage0);
            Assert.IsNotEmpty(assetsPage0);

            IEnumerable<IImageAsset> assetsPage1 = await ClientService.GetMainGalery(1);

            Assert.NotNull(assetsPage1);
            Assert.IsNotEmpty(assetsPage1);

            Assert.AreNotSame(assetsPage0, assetsPage1);
            Assert.AreNotEqual(assetsPage0, assetsPage1);
        }

        [Test]
        public async Task ImageMetadata()
        {
            IEnumerable<IImageAsset> assets = await ClientService.GetMainGalery(0);

            Assert.NotNull(assets);
            Assert.IsNotEmpty(assets);

            var asset = assets.First(imageAsset => imageAsset.ShouldDisplay);

            Assert.NotNull(assets);

            Assert.NotNull(asset.Title);
            Assert.IsNotEmpty(asset.Title);

            Assert.NotNull(asset.Id);
            Assert.IsNotEmpty(asset.Id);

            Assert.NotZero(asset.Ratio);
        }

        [Test]
        public async Task ImageComments()
        {
            IEnumerable<IImageAsset> assets = await ClientService.GetMainGalery(0);

            Assert.NotNull(assets);
            Assert.IsNotEmpty(assets);

            var asset = assets.First(imageAsset => imageAsset.ShouldDisplay);

            Assert.NotNull(assets);

            Assert.NotNull(asset.Title);
            Assert.NotNull(asset.Id);
            Assert.IsNotEmpty(asset.Id);

            Assert.NotZero(asset.Ratio);

            try
            {
                IEnumerable<IAssetComment> comments = await ClientService.GetGalleryAssetComments(asset);
                if (comments == null || !comments.Any())
                    return;

                var comment = comments.First();

                Assert.IsNotEmpty(comment.Author);
                Assert.IsNotEmpty(comment.Comment);
            }
            catch (Exception e)
            {
            }
        }

        [Test]
        public async Task MainSearchGelleryDownload()
        {
            IEnumerable<IImageAsset> assets = await ClientService.SearchMainGalery("Kreia", 0);

            Assert.NotNull(assets);
            Assert.IsNotEmpty(assets);
        }

        [Test]
        public async Task ImageSearchMainGalleryPaging()
        {
            IEnumerable<IImageAsset> assetsPage0 = await ClientService.SearchMainGalery("Kreia", 0);

            Assert.NotNull(assetsPage0);
            Assert.IsNotEmpty(assetsPage0);

            IEnumerable<IImageAsset> assetsPage1 = await ClientService.SearchMainGalery("Kreia", 1);

            Assert.NotNull(assetsPage1);
            Assert.IsNotEmpty(assetsPage1);

            Assert.AreNotSame(assetsPage0, assetsPage1);
            Assert.AreNotEqual(assetsPage0, assetsPage1);
        }
    }
}
