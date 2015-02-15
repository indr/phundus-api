namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Application.Commands;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (ChangeQuantityInInventoryHandler))]
    public class when_change_quantity_in_inventory_is_handled :
        stock_concern<ChangeQuantityInInventory, ChangeQuantityInInventoryHandler>
    {
        private static int _quantity = 10;
        private static Period _period = new Period(DateTime.UtcNow);
        private static string _comment = "Comment";

        private Establish ctx =
            () =>
                command = new ChangeQuantityInInventory(_initiatorId, _organizationId, _articleId, _stockId, _quantity,
                    _period.FromUtc, _comment);


        public It should_ask_for_chief_privileges =
            () =>
                _memberInRole.WasToldTo(
                    x =>
                        x.ActiveChief(Arg<OrganizationId>.Is.Equal(_organizationId), Arg<UserId>.Is.Equal(_initiatorId)));

        public It should_call_change_quantity = ()
            => _stock.WasToldTo(x => x.ChangeQuantityInInventory(_period, _quantity, _comment));

        public It should_save_to_repository = () => _repository.WasToldTo(x => x.Save(_stock));
    }
}