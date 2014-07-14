namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;

    public interface IMemberService
    {
        Member MemberFrom(int id);
    }
}