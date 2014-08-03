namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Orders.Model;
    using ReservationCtx.Model;

    public class OrderDtoAssembler
    {
        public OrderDto CreateDto(Order subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new OrderDto
                             {
                                 Id = subject.Id,
                                 Version = subject.Version,
                                 CreateDate = subject.CreateDate,
                                 ModifyDate = subject.ModifyDate,
                                 TotalPrice = subject.TotalPrice,
                                 Status = subject.Status
                       };

            if (subject.Reserver != null)
            {
                result.ReserverId = subject.Reserver.Id;
                result.ReserverName = subject.Reserver.DisplayName;
            }

            if (subject.Modifier != null)
            {
                result.ModifierId = subject.Modifier.Id;
                result.ModifierName = subject.Modifier.DisplayName;
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
    