namespace Phundus.Tests
{
    using Rhino.Mocks;
    
    // ReSharper disable InconsistentNaming
    public class Mock
    {
        public T mock<T>() where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        public T strict<T>(object[] argumentsForConstructor)
        {
            return MockRepository.GenerateStrictMock<T>(argumentsForConstructor);
        }

        public T partial<T>(object[] argumentsForConstructor)
        {
            return MockRepository.GeneratePartialMock<T>(argumentsForConstructor);
        }
    }
    // ReSharper restore InconsistentNaming
}