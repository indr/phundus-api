using System;
using WatiN.Core;

namespace phiNdus.fundus.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public Browser Browser { get; set; }
        public string BaseUri { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            Browser.Dispose();
        }

        #endregion
    }
}