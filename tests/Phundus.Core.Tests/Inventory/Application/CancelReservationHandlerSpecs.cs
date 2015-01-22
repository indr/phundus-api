namespace Phundus.Core.Tests.Inventory.Application
{
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class when_cancel_reservation_is_handled : handler_concern<CancelReservation, CancelReservationHandler>
    {
        private static UserId _initiatorId = new UserId(1001);
        private static OrganizationId _organizationId = new OrganizationId(1001);
        private static ReservationId _reservationId = new ReservationId();
        private static ArticleId _articleId = new ArticleId(10001);
        private static OrderId _orderId = new OrderId(101);
        private static Period _period = Period.FromTodayToTomorrow;
        private static int _quantity = 1;
        
        private static Reservation _reservation;
        private static IMemberInRole _memberInRole;
        private static IReservationRepository _reservationRepository;

        private Establish ctx = () =>
        {
            _reservation = MockRepository.GenerateStub<Reservation>(new object[] {_reservationId, _organizationId, _articleId, _orderId, _period, _quantity});

            _memberInRole = depends.on<IMemberInRole>();

            _reservationRepository = depends.on<IReservationRepository>();
            _reservationRepository.WhenToldTo(x => x.Get(_reservationId)).Return(_reservation);
            
            command = new CancelReservation(_initiatorId, _organizationId, _reservationId);
        };

        public It should_ask_for_chief_privileges =
            () => _memberInRole.WasToldTo(x => x.ActiveChief(_organizationId, _initiatorId));

        public It should_cancel_reservation = () => _reservation.WasToldTo(x => x.Cancel());

        public It should_save_to_repository = () => _reservationRepository.WasToldTo(x => x.Save(_reservation));
    }
}