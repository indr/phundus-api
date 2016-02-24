namespace Phundus.Tests.Inventory
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Model;

    public class inventory_factory : factory_base
    {
        public inventory_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Article Article(Owner owner = null)
        {
            var article = fake.an<Article>();
            var articleId = new ArticleShortId(NextNumericId());
            article.setup(x => x.Id).Return(articleId.Id);
            article.setup(x => x.ArticleShortId).Return(articleId);
            article.setup(x => x.ArticleId).Return(new ArticleId());
            article.setup(x => x.Owner).Return(owner ?? Owner());
            return article;
        }

        public Owner Owner(OwnerType ownerType = OwnerType.Organization)
        {
            return new Owner(new OwnerId(), "The Owner", ownerType);
        }

        public Store Store(Owner owner = null)
        {
            owner = owner ?? Owner();
            var store = fake.an<Store>();
            store.setup(x => x.Id).Return(new StoreId());
            store.setup(x => x.Owner).Return(owner);
            return store;
        }

        public Manager Manager()
        {
            return new Manager(new UserId(), "manager@test.phundus.ch", "The Manager");
        }
    }
}