namespace Phundus.Tests.IdentityAccess.Model.Users.Mails
{
    using Machine.Specifications;
    using Phundus.IdentityAccess.Model.Users;

    public abstract class identityaccess_mail_concern<TMail> : mail_concern<TMail> where TMail : class
    {
        protected static identityaccess_factory make;

        protected static Admin theAdmin;

        private Establish ctx = () =>
        {
            make = new identityaccess_factory(fake);
            theAdmin = make.Admin();
        };
    }
}