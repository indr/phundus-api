namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Orders.Model;

    public enum OrderStatusDto
    {
        Pending = 1,
        Approved,
        Rejected,
        Closed
    }

    public class OrderDtoAssembler
    {
        public LegacyOrderDto CreateDto(Order subject)
        {
            if (subject == null)
                return null;

            var result = new LegacyOrderDto
            {
                Id = subject.Id,
                Version = subject.Version,
                OrganizationId = subject.Organization.Id,
                CreateDate = subject.CreatedUtc.ToLocalTime(),
                ModifiedOn = subject.ModifiedUtc.HasValue ? subject.ModifiedUtc.Value.ToLocalTime() : (DateTime?)null,
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
                result.Items.Add(new LegacyOrderItemDto
                {
                    Amount = item.Amount,
                    ArticleId = item.Article.Id,
                    From = item.FromUtc.ToLocalTime(),
                    Id = item.Id,
                    OrderId = item.Order.Id,
                    To = item.ToUtc.ToLocalTime(),
                    UnitPrice = item.UnitPrice,
                    LineTotal = item.LineTotal,
                    Version = item.Version,
                    Text = item.Article.Caption
                });
            }

            return result;
        }

        public IList<LegacyOrderDto> CreateDtos(IEnumerable<Order> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<LegacyOrderDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result;
        }
    }
}