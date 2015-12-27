namespace Phundus.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using Core.IdentityAndAccess.Queries;
    using phiNdus.fundus.Web.Helpers.FileUpload;

    public class FilesController : ControllerBase
    {
        public IOrganizationRepository OrganizationRepository { get; set; }
        public IMemberInRole MemberInRole { get; set; }

        private string GetPath(Guid orgId)
        {
            var organization = OrganizationRepository.GetById(orgId);
            return String.Format(@"~\Content\Uploads\Organizations\{0}", organization.Id);
        }

        private ImageStore CreateImageStore(string path)
        {
            return new ImageStore(path);
        }

        private BlueImpFileUploadJsonResultFactory CreateFactory(string path)
        {
            var factory = new BlueImpFileUploadJsonResultFactory();
            factory.ImageUrl = Url.Content(path);
            factory.DeleteUrl = Url.Action("Delete", "Files");
            return factory;
        }

        //
        // GET: /Files/
        [HttpGet]
        [Transaction]
        public virtual JsonResult Index(Guid orgId)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var path = GetPath(orgId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(path);
            var files = store.GetFiles();
            return Json(factory.Create(files), JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        [Transaction]
        public virtual JsonResult Delete(Guid orgId, string id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var path = GetPath(orgId);
            var store = CreateImageStore(path);
            store.Delete(id);
            return Json("");
        }

        [HttpPost]
        [Transaction]
        public virtual JsonResult Index(Guid orgId, string id)
        {
            MemberInRole.ActiveChief(orgId, CurrentUserId);

            var path = GetPath(orgId);
            var store = CreateImageStore(path);
            var factory = CreateFactory(path);
            var handler = new BlueImpFileUploadHandler(store);
            var images = handler.Post(HttpContext.Request.Files);
            var result = factory.Create(images);
            return Json(result);
        }
    }
}