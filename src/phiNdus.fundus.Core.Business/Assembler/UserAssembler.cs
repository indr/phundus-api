using System;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;


namespace phiNdus.fundus.Core.Business.Assembler
{
    public class UserAssembler
    {
        public UserDto WriteDto(User subject)
        {
            // TODO: Use Guard.Against
            if (subject == null)
                throw new ArgumentNullException("subject");
            
            var result = new UserDto();

            result.Id = subject.Id;
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;
            result.Email = subject.Membership.Email;

            WriteMembership(result, subject.Membership);

            return result;
        }

        private static void WriteMembership(UserDto result, Membership subject)
        {
            result.CreateDate = subject.CreateDate;
            result.IsApproved = subject.IsApproved;
            result.PasswordQuestion = subject.PasswordQuestion;
        }
    }
}