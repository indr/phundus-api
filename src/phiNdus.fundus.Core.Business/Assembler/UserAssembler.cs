﻿using System;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;


namespace phiNdus.fundus.Core.Business.Assembler
{
    public class UserAssembler
    {
        public static UserDto WriteDto(User subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            
            var result = new UserDto();

            result.Id = subject.Id;
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            WriteMembership(result, subject.Membership);

            return result;
        }

        private static void WriteMembership(UserDto result, Membership subject)
        {
            Guard.Against<ArgumentNullException>(subject == null, "subject");
            result.Email = subject.Email;
            result.CreateDate = subject.CreateDate;
            result.IsApproved = subject.IsApproved;
        }
    }
}