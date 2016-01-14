﻿namespace Phundus.Shop.Orders
{
    using System.IO;
    using IdentityAccess.Queries;
    using Model;
    using Queries;
    using Repositories;
    using Services;

    public interface IPdfStore
    {
        Stream GetOrderPdf(int orderId, int currentUserId);
    }

    public class PdfStore : IPdfStore
    {
        public IMemberInRole MemberInRole { get; set; }

        public IOrderRepository OrderRepository { get; set; }

        public IOrderQueries OrderQueries { get; set; }

        public IOrderPdfGeneratorService OrderPdfGeneratorService { get; set; }

        public Stream GetOrderPdf(int orderId, int currentUserId)
        {
            var order = OrderRepository.GetById(orderId);
            return GetPdf(order);
        }

        private Stream GetPdf(Order order)
        {
            return OrderPdfGeneratorService.GeneratePdf(order);
        }
    }
}