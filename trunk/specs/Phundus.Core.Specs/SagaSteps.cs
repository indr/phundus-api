namespace Phundus.Core.Specs
{
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class SagaSteps
    {
        private readonly SagaContext _context;

        public SagaSteps(SagaContext context)
        {
            _context = context;
        }

        [Then(@"no commands dispatched")]
        public void ThenNoCommandsDispatched()
        {
            Assert.That(_context.Saga.UndispatchedCommands.Count, Is.EqualTo(0));
        }
    }
}