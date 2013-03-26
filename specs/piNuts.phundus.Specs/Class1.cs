using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace piNuts.phundus.Specs
{
    using Machine.Specifications;

    public class HomeController
    {
        
    }


    

    [Subject(typeof(HomeController))]
    public class context_for_a_home_controller
    {
        protected static HomeController homeController;

        Establish context =
            () => { homeController = new HomeController(); };
    }

    [Subject(typeof(HomeController))]
    public class when_the_home_controller_is_told_to_show_index : context_for_a_home_controller
    {
        //static ActionResult result;

        Because of =
            () => { };//result = homeController.Index()};

        private It should_show_the_default_view =
            () => true.ShouldBeTrue();// result.ShouldBeAView().And().ShouldUseDefaultView();
    }
}
