namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Orders.Model;

    public class OrderDtoAssembler
    {
        public OrderDto CreateDto(Order subject)
        {
            if (subject == null)
                return null;

            var result = new OrderDto
            {
                Id = subject.Id,
                Version = subject.Version,
                OrganizationId = subject.OrganizationId,
                CreateDate = subject.CreatedOn,
                ModifiedOn = subject.ModifiedOn,
                TotalPrice = subject.TotalPrice,
                Status = subject.Status
            };

            if (subject.Borrower != null)
            {
                result.Borrower = new BorrowerDto
                {
                    BorrowerId = subject.Borrower.Id,
                    EmailAddress = subject.Borrower.EmailAddress,
                    FirstName = subject.Borrower.FirstName,
                    LastName = subject.Borrower.LastName,
                    MemberNumber = subject.Borrower.MemberNumber
                };
            }

            foreach (var item in subject.Items)
            {
                result.Items.Add(new OrderItemDto
                {
                    Amount = item.Amount,
                    ArticleId = item.Article.Id,
                    From = item.From,
                    Id = item.Id,
                    OrderId = item.Order.Id,
                    To = item.To,
                    UnitPrice = item.UnitPrice,
                    LineTotal = item.LineTotal,
                    Version = item.Version,
                    Text = item.Article.Caption
                });
            }

            return result;
        }

        public IList<OrderDto> CreateDtos(IEnumerable<Order> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<OrderDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result;
        }
    }
}