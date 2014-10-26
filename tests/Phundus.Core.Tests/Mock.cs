namespace Phundus.Core.Tests
{
    using Rhino.Mocks;

    public class Mock
    {
        public T strict<T>()
        {
            return strict<T>(new object[0]);
        }

        public T strict<T>(object[] argumentsForConstructor)
        {
            return MockRepository.GenerateStrictMock<T>(argumentsForConstructor);
        }

        public T stub<T>() where T : class
        {
            return stub<T>(new object[0]);
        }

        public T stub<T>(object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateStub<T>(argumentsForConstructor);
        }

        public T partial<T>()
        {
            return partial<T>(new object[0]);
        }

        public T partial<T>(object[] argumentsForConstructor)
        {
            return MockRepository.GeneratePartialMock<T>(argumentsForConstructor);
        }
    }
}