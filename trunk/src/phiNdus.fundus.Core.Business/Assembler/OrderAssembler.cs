using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Assembler
{
    public class OrderAssembler
    {
        public static OrderDto CreateDto(Order subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");

            var result = new OrderDto
                       {
                           Id = subject.Id,
                           Version = subject.Version,
                           CreateDate = subject.CreateDate,
                           ModifyDate = subject.ModifyDate
                           
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

            return result;
        }

        public static IList<OrderDto> CreateDtos(ICollection<Order> subjects)
        {
            Guard.Against<ArgumentNullException>(subjects == null, "subjects");

            var result = new List<OrderDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result;
        }
    }
}
    