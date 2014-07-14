namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;
    using IdentityAndAccessCtx.Repositories;

    public class UserRepositoryAdapter
    {
        public IUserRepository Users { get; set; }

        public Member ToMember(int id)
        {
            var user = Users.FindById(id);

            return new Member(user.Id, user.FirstName, user.LastName);
        }
    }
}