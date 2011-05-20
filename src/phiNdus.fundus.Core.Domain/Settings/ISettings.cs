using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.Core.Domain.Settings
{
    public interface ISettings
    {
        IMailSettings Mail { get; }
    }
}
