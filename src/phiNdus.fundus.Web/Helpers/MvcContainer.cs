using System;
using System.Web.Mvc;

namespace phiNdus.fundus.Web.Helpers
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

    public class MvcFieldSet : IDisposable
    {
        private readonly ViewContext _viewContext;

        public MvcFieldSet(ViewContext viewContext, string id, string legend)
        {
            _viewContext = viewContext;
            _viewContext.Writer.WriteLine("<fieldset id=\"{0}\"><legend>{1}</legend>", id, legend);
        }

        public void Dispose()
        {
            _viewContext.Writer.WriteLine("</fieldset>");
        }
    }
}
