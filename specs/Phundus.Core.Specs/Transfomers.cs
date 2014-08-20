using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Core.Specs
{
    using TechTalk.SpecFlow;

    [Binding]
    public class Transforms
    {
        [StepArgumentTransformation(@"([\d\.]+) CHF")]
        public decimal ChfCurrency(string value)
        {
            return Convert.ToDecimal(value);
        }
    }
}
