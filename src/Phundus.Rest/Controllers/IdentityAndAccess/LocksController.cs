namespace Phundus.Rest.Controllers.IdentityAndAccess
{
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;

    public class LocksController : ApiControllerBase
    {
        [Transaction]
        public virtual void Post(int organizationId, int memberId)
        {
            Dispatcher.Dispatch(new LockMember
            {
                InitiatorId = CurrentUserId,
                MemberId = memberId,
                OrganizationId = organizationId
            });
        }

        [Transaction]
        public virtual void Delete(int organizationId, int memberId)
        {
            Dispatcher.Dispatch(new UnlockMember
            {
                InitiatorId = CurrentUserId,
                MemberId = memberId,
                OrganizationId = organizationId
            });
        }
    }
}