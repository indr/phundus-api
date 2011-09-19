using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Business.Services;

namespace phiNdus.fundus.Core.Business.SecuredServices
{
    public class SecuredArticleService : BaseSecuredService, IArticleService
    {
        #region IArticleService Members

        public ArticleDto GetArticle(string sessionKey, int id)
        {
            return Secured.With(Session.FromKey(sessionKey))
                .Do<ArticleService, ArticleDto>(svc => svc.GetArticle(id));
        }

        #endregion
    }
}