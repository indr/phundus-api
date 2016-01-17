namespace Phundus.Tests
{
    using System;
    using developwithpassion.specifications.core;

    public class factory_base
    {
        private Random _random;
        protected ICreateFakes fake;

        public factory_base(ICreateFakes fake)
        {
            _random = new Random();
            this.fake = fake;
        }

        protected int NextNumericId()
        {
            return _random.Next(0, int.MaxValue);
        }
    }
}