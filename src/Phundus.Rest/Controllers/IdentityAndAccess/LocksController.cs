namespace Phundus.Rest.Controllers.IdentityAndAccess
{
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;

    public class LocksController : ApiControllerBase
    {
        public IUserQueries UserQueries { get; set; }

        [Transaction]
        public virtual void Post(int organizationId, int memberId)
        {
            var chiefId = UserQueries.ByEmail(Identity.Name).Id;

            Dispatcher.Dispatch(new LockMember
            {
                ChiefId = chiefId,
                MemberId = memberId,
                OrganizationId = organizationId
            });
        }

        [Transaction]
        public virtual void Delete(int organizationId, int memberId)
        {
            var chiefId = UserQueries.ByEmail(Identity.Name).Id;

            Dispatcher.Dispatch(new UnlockMember
            {
                ChiefId = chiefId,
                MemberId = memberId,
                OrganizationId = organizationId
            });
        }
    }
}