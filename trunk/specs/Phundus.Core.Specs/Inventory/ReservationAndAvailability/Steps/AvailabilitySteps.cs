﻿namespace Phundus.Core.Specs.Inventory.ReservationAndAvailability.Steps
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using NUnit.Framework;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Articles.Repositories;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Reservations;
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
        public void GivenAnArticleWithGrossStockOf(int quantity)
        {
            var owner = new Owner(new OwnerId(Guid.NewGuid()), "Owner", OwnerType.Organization);
            _article = new Article(owner, new StoreId(), new ArticleId(), "Name", 0, 1.11m, null);
            _article.GrossStock = quantity;
            _articleRepository.Stub(x => x.FindById(Arg<ArticleId>.Is.Equal(_article.ArticleId))).Return(_article);
        }

        [When(@"I ask for availability details")]
        public void WhenIAskForAvailabilityDetails()
        {
            _reservationRepository.Stub(x => x.Find(_article.ArticleId, null)).Return(_reservations);
            _availabilities = Sut.GetAvailabilityDetails(_article.ArticleId);
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
            _reservationRepository.Stub(x => x.Find(_article.ArticleId, null)).Return(_reservations);
            _isAvailable = Sut.IsArticleAvailable(_article.ArticleId, from, to, of, null);
        }

        [Then(@"the result should be (true|false)")]
        public void ThenTheResultShouldBeTrue(bool trueOrFalse)
        {
            Assert.That(_isAvailable, Is.EqualTo(trueOrFalse));
        }
    }
}