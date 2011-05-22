using System;
using phiNdus.fundus.Core.Business.Security;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Services
{
    public class BaseService
    {
        private bool _sessionSet;
        private Session _session;

        public Session Session
        {
            get { return _session; }
            set
            {
                Guard.Against<InvalidOperationException>(_sessionSet, "Session can only be set once");
                _sessionSet = true;
                _session = value;
            }
        }
    }
}