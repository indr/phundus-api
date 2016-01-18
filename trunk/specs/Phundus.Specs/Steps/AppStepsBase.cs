namespace Phundus.Specs.Steps
{
    using System;
    using Services;
    using TechTalk.SpecFlow;

    public abstract class AppStepsBase : Steps
    {
        protected readonly App App;
        protected readonly Ctx Ctx;

        protected AppStepsBase(App app, Ctx ctx)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (ctx == null) throw new ArgumentNullException("ctx");
            App = app;
            Ctx = ctx;
        }
    }
}