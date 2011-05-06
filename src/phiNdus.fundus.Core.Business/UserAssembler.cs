using System;
using phiNdus.fundus.Core.Domain;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    public class UserAssembler
    {
        public UserDto WriteDto(User subject)
        {
            var result = new UserDto();

            result.Id = subject.Id;
            result.FirstName = subject.FirstName;
            result.LastName = subject.LastName;

            WriteMembership(result, subject.Membership);

            return result;
        }

        private void WriteMembership(UserDto result, Membership subject)
        {
            result.CreateDate = subject.CreateDate;
            result.IsApproved = subject.IsApproved;
            result.PasswordQuestion = subject.PasswordQuestion;
        }
    }
}