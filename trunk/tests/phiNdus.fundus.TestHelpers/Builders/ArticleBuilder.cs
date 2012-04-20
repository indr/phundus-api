using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class ArticleBuilder : BuilderBase<Article>
    {
        public override Article Build()
        {
            var result = new Article();
            Persist(result);
            return result;
        }
    }
}