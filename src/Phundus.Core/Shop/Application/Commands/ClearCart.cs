﻿namespace Phundus.Shop.Application
{
    using System;
    using Castle.Transactions;
    using Common.Commanding;
    using Common.Domain.Model;
    using Shop.Model;

    public class ClearCart : ICommand
    {
        public ClearCart(InitiatorId initiatorId)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            InitiatorId = initiatorId;
        }

        public InitiatorId InitiatorId { get; protected set; }
    }

    public class ClearCartHandler : IHandleCommand<ClearCart>
    {
        private readonly ICartRepository _cartRepository;

        public ClearCartHandler(ICartRepository cartRepository)
        {
            if (cartRepository == null) throw new ArgumentNullException("cartRepository");
            _cartRepository = cartRepository;
        }

        [Transaction]
        public void Handle(ClearCart command)
        {
            var cart = _cartRepository.FindByUserGuid(new UserId(command.InitiatorId.Id));
            if (cart == null)
                return;

            cart.Clear();
        }
    }
}