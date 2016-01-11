namespace Phundus.Persistence.Tests
{
    using developwithpassion.specifications.rhinomocks;

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        
    }
}