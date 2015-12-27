namespace Phundus.Core.Specs.Inventory.ReservationAndAvailability.Steps
{
    using System;
    using System.Collections.Generic;
    using Core.Inventory.Articles.Model;
    using Core.Inventory.Articles.Repositories;
    using Core.Inventory.AvailabilityAndReservation.Model;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Core.Inventory.Owners;
    using Core.Inventory.Services;
    using NUnit.Framework;
    using Rhino.Mocks;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class AvailabilitySteps : concern<AvailabilityService>
    {
        private Article _article;
        private IEnumerable<Availability> _availabilities;
        private bool _isAvailable;
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
            var owner = new Owner(new OwnerId(Guid.NewGuid()), "Owner");
            _article = new Article(1, owner, "Name");
            _article.GrossStock = amount;
            _articleRepository.Stub(x => x.FindById(_article.Id)).Return(_article);
        }

        [When(@"I ask for availability details")]
        public void WhenIAskForAvailabilityDetails()
        {
            _reservationRepository.Stub(x => x.Find(_article.Id, Guid.Empty)).Return(_reservations);
            _availabilities = Sut.GetAvailabilityDetails(_article.Id);
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

        [When(@"I ask for availability from (.*?) to (.*?) of (.*)")]
        public void WhenIAskForAvailabilityFrom_To_Of(DateTime from, DateTime to, int of)
        {
            _reservationRepository.Stub(x => x.Find(_article.Id, Guid.Empty)).Return(_reservations);
            _isAvailable = Sut.IsArticleAvailable(_article.Id, from, to, of, Guid.Empty);
        }

        [Then(@"the result should be (true|false)")]
        public void ThenTheResultShouldBeTrue(bool trueOrFalse)
        {
            Assert.That(_isAvailable, Is.EqualTo(trueOrFalse));
        }
    }
}