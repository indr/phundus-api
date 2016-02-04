namespace Phundus.Specs.Features.Articles
{
    using ContentTypes;
    using Services;
    using Services.Entities;
    using Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ArticleFilesSteps : AppStepsBase
    {
        private FileUploadResponseContent _fileUploadResponseContent;
        private Files _files;

        public ArticleFilesSteps(App app, Ctx ctx, Files files, ApiClient apiClient)
            : base(app, ctx)
        {
            _files = files;
        }

        [Given(@"I uploaded an article image (.+)")]
        public void GivenIUploadedAnArticleImage(string fileName)
        {
            UploadArticleImage(fileName);
        }

        [Given(@"I uploaded an article document (.+)")]
        public void GivenIUploadedAnArticleDocument(string fileName)
        {
            UploadArticleDocument(fileName);
        }

        [Given(@"I set (.+) as the preview image")]
        public void GivenISetAsThePreviewImage(string fileName)
        {
            App.SetArticlePreviewImage(Ctx.Article, fileName);
        }

        [When(@"I try to upload an article image")]
        public void WhenITryToUploadAnArticleImage()
        {
            UploadArticleImage("");
        }

        [Given(@"I uploaded an article image")]
        [When(@"I try to upload an article image (.+)")]
        public void WhenITryToUploadAnArticleImage(string fileName)
        {
            UploadArticleImage(fileName);
        }

        private void UploadArticleImage(string fileName)
        {
            var filePath = _files.GetNextImageFileName();
            Ctx.FileNames[fileName] = filePath;
            _fileUploadResponseContent = App.UploadArticleImage(Ctx.Article, filePath, fileName);
        }

        private void UploadArticleDocument(string fileName)
        {
            var filePath = _files.GetNextDocumentFileName();
            Ctx.FileNames[fileName] = filePath;
            _fileUploadResponseContent = App.UploadArticleDocument(Ctx.Article, filePath, fileName);
        }

        [When(@"I try to get the article images")]
        public void WhenITryToGetTheArticleImages()
        {
            _fileUploadResponseContent = App.GetArticleFiles(Ctx.Article);
        }

        [Then(@"I should get file upload response content")]
        public void ThenIShouldGetFileUploadResponseContent(Table table)
        {
            table.CompareToSet(_fileUploadResponseContent.Files);
        }
    }
}