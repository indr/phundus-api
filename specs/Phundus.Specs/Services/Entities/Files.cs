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
    }
}