using System;
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
                           ReserverId = subject.Reserver.Id,
                           ReserverName = subject.Reserver.DisplayName,
                           ApproveDate = subject.ApproveDate,
                           RejectDate = subject.RejectDate
                       };
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
    }
}
    