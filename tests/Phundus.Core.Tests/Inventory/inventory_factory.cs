namespace Phundus.Tests.Inventory
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Stores.Model;

    public class inventory_factory : factory_base
    {
        public inventory_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Article Article(Owner owner = null)
        {
            var article = fake.an<Article>();
            var articleId = new ArticleId(NextNumericId());
            article.setup(x => x.Id).Return(articleId.Id);
            article.setup(x => x.ArticleId).Return(articleId);
            article.setup(x => x.Owner).Return(owner ?? Owner());
            return article;
        }

        public Owner Owner()
        {
            var owner = fake.an<Owner>();
            owner.setup(x => x.OwnerId).Return(new OwnerId());
            return owner;
        }

        public Store Store()
        {
            var store = fake.an<Store>();
            store.setup(x => x.Id).Return(new StoreId());
            return store;
        }
    }
}