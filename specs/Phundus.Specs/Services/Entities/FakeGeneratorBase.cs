namespace Phundus.Specs.Services.Entities
{
    using System;
    using System.Collections.Generic;

    public abstract class FakeGeneratorBase<TRecord> where TRecord : class
    {
        private int _nextIdx;
        private readonly IList<TRecord> _records;
        private int _used;
        private readonly bool _useOnlyOnce;
        protected Random Random;

        protected FakeGeneratorBase(string resourceName, bool userOnlyOnce = false)
        {
            _records = RecordsReader.ReadFromResource<TRecord>(resourceName);
            _nextIdx = new Random().Next(0, _records.Count - 1);
            _useOnlyOnce = userOnlyOnce;
            Random = new Random();
        }

        protected TRecord GetNextRecord()
        {
            if (_useOnlyOnce && _used >= _records.Count)
                throw new InvalidOperationException(
                    String.Format(
                        @"You have used {0} of {1} fake records. Get a bigger file or disable the use only once option.",
                        _used, _records.Count));

            var result = _records[_nextIdx++];

            if (_nextIdx >= _records.Count)
                _nextIdx = 0;

            _used++;
            return result;
        }
    }
}