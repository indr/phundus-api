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
                           ApproveDate = subject.ApproveDate,
                           RejectDate = subject.RejectDate
                       };
            if (subject.Reserver != null)
            {
                result.ReserverId = subject.Reserver.Id;
                result.ReserverName = subject.Reserver.DisplayName;
            }

            if (subject.Approver != null)
            {
                result.ApproverId = subject.Approver.Id;
                result.ApproverName = subject.Approver.DisplayName;
            }

            if (subject.Rejecter != null)
            {
                result.RejecterId = subject.Rejecter.Id;
                result.RejecterName = subject.Rejecter.DisplayName;
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
    