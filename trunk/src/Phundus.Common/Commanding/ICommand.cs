namespace Phundus.Common.Commanding
{
    using System;

    public interface ICommand
    {
    }

    public abstract class AsyncCommand : ICommand
    {
        private readonly Guid _commandId = Guid.NewGuid();
        private readonly DateTime _createdAtUtc = DateTime.UtcNow;

        public Guid CommandId
        {
            get { return _commandId; }
        }

        public DateTime CreatedAtUtc
        {
            get { return _createdAtUtc; }
        }
    }
}