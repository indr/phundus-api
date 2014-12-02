namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (ReserveArticleHandler))]
    public class when_create_reservation_is_handled : handler_concern<ReserveArticle, ReserveArticleHandler>
    {
        private static IReservationRepository repository;
        private static IMemberInRole memberInRole;
        private static OrderId orderId = new OrderId(1);
        private static CorrelationId correlationId = new CorrelationId(Guid.NewGuid());

        public Establish ctx = () =>
        {
            memberInRole = depends.on<IMemberInRole>();

            repository = depends.on<IReservationRepository>();
            repository.WhenToldTo(x => x.GetNextIdentity()).Return(new ReservationId("R_1234"));

            command = new ReserveArticle(1, 101, 201, orderId, correlationId, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), 1);
        };

        public It should_ask_for_member_privileges =
            () => memberInRole.WasToldTo(x => x.ActiveMember(Arg<OrganizationId>.Matches(p => p.Id == 101),
                Arg<UserId>.Matches(p => p.Id == 1)));

        public It should_have_resulting_reservation_id = () => command.ResultingReservationId.ShouldEqual("R_1234");

        public It should_save_to_repository = () => repository.WasToldTo(x => x.Save(Arg<Reservation>.Is.NotNull));
    }
}