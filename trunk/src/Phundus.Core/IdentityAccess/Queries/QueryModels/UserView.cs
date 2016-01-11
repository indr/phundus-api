namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System.Collections.Generic;
    using Core.Cqrs;

    public interface IUserView
    {
        IEnumerable<UserViewRow> Query();
    }

    public class UserView : ReadModelBase, IUserView
    {
        public IEnumerable<UserViewRow> Query()
        {
            return Session.QueryOver<UserViewRow>().List();
        }
    }
}