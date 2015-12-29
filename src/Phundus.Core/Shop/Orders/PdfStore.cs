﻿namespace Phundus.Core.Shop.Orders
{
    using System;
    using System.IO;
    using IdentityAndAccess.Queries;
    using Model;
    using Repositories;
    using Services;

    public interface IPdfStore
    {
        Stream GetOrderPdf(int orderId, int currentUserId);
        Stream GetOrderPdf(int orderId, Guid organizationId, int currentUserId);
    }

    public class PdfStore : IPdfStore
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public Stream GetOrderPdf(int orderId, int currentUserId)
        {
            var order = OrderRepository.GetById(orderId);
            if (order.Borrower.Id != currentUserId)
                return null;

            return GetPdf(order);
        }

        public Stream GetOrderPdf(int orderId, Guid organizationId, int currentUserId)
        {
            MemberInRole.ActiveChief(organizationId, currentUserId);

            var order = OrderRepository.GetById(orderId);
            if (order.Lessor.LessorId != organizationId)
                return null;

            return GetPdf(order);
        }

        private Stream GetPdf(Order order)
        {
            return OrderPdfGeneratorService.GeneratePdf(order);
        }
    }
}