using System;
using System.Web.Mvc;

namespace phiNdus.fundus.Core.Web.Helpers
{
    public class MvcContainer : IDisposable
    {
        private readonly ViewContext _viewContext;

        public MvcContainer(ViewContext viewContext, string id)
        {
            _viewContext = viewContext;
            _viewContext.Writer.WriteLine("<div id=\"{0}\">", id);
        }

        #region IDisposable Members

        public void Dispose()
        {
            EndContainer();
        }

        #endregion

        public void EndContainer()
        {
            _viewContext.Writer.WriteLine("</div>");
        }
    }
}
