namespace Phundus.Specs.Steps
{
    using System;
    using Services;

    public abstract class StepsBase
    {
        protected readonly App App;
        protected readonly Ctx Ctx;

        protected StepsBase(App app, Ctx ctx)
        {
            if (app == null) throw new ArgumentNullException("app");
            if (ctx == null) throw new ArgumentNullException("ctx");
            App = app;
            Ctx = ctx;
        }
    }
}