using System;

namespace phiNdus.fundus.Core.Business
{
    public class DtoOutOfDateException : Exception
    {
        public DtoOutOfDateException(string message) : base(message)
        {
        }
    }
}