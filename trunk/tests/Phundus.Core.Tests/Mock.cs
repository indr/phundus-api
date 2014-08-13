namespace Phundus.Core.Tests
{
    using Rhino.Mocks;

    public class Mock
    {
        public T strict<T>(object[] argumentsForConstructor)
        {
            return MockRepository.GenerateStrictMock<T>(argumentsForConstructor);
        }

        public T partial<T>(object[] argumentsForConstructor)
        {
            return MockRepository.GeneratePartialMock<T>(argumentsForConstructor);
        }
    }
}