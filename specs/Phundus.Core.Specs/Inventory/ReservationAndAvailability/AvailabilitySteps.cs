namespace Phundus.Core.Specs.Inventory.ReservationAndAvailability
{
    using System.Collections.Generic;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Articles.Repositories;
    using Core.Inventory.AvailabilityAndReservation.Model;
    using Core.Inventory.AvailabilityAndReservation._Legacy;
    using Core.Inventory.Services;
    using ReservationCtx.Repositories;
    using Rhino.Mocks;
    using Rhino.Mocks.Constraints;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AvailabilitySteps : concern<AvailabilityService>
    {
        private Article _article;
        private IEnumerable<Availability> _availabilities;
        private readonly IArticleRepository _articleRepository;
        private readonly IReservationRepository _reservationRepository;
        private IEnumerable<Reservation> _reservations = new List<Reservation>();

        public AvailabilitySteps()
        {
            _articleRepository = dependsOn<IArticleRepository>();
            _reservationRepository = dependsOn<IReservationRepository>();
        }

        [Given(@"an article with gross stock of (.*)")]
        public void GivenAnArticleWithGrossStockOf(int amount)
        {
            _article = new Article(1001, "Name");
            _article.GrossStock = amount;
            _articleRepository.Stub(x => x.GetById(_article.Id)).Return(_article);
        }

        [When(@"I ask for availability")]
        public void WhenIAskForAvailability()
        {
            _reservationRepository.Stub(x => x.Find(_article.Id)).Return(_reservations);
            _availabilities = Sut.GetAvailability(_article.Id);
        }

        [Given(@"these reservations exists")]
        public void GivenTheseReservationsExists(Table table)
        {
            _reservations = table.CreateSet<Reservation>();
        }

        [Then(@"the result should be")]
        public void ThenTheResultShouldBe(Table table)
        {
            table.CompareToSet(_availabilities);
        }
    }
}