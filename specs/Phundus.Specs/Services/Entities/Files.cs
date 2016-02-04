namespace Phundus.Specs.Services.Entities
{
    using System;
    using System.IO;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class Files
    {
        private const string AssetsImages = @".\Assets\Images\";
        private const string AssetsDocuments = @".\Assets\Documents\";
        private readonly Random _random;

        public Files()
        {
            _random = new Random();
        }

        public string GetNextImageFileName()
        {
            var images = Directory.GetFiles(AssetsImages);
            return images[_random.Next(0, images.Count() - 1)];
        }

        public string GetNextDocumentFileName()
        {
            var documents = Directory.GetFiles(AssetsDocuments);
            return documents[_random.Next(0, documents.Count() - 1)];
        }
    }
}