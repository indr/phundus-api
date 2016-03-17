namespace Phundus.Tests.Inventory
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.Inventory.Articles.Model;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Stores;

    public class inventory_factory : factory_base
    {
        public inventory_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Article Article(Owner owner = null)
        {
            owner = owner ?? Owner();
            var article = fake.an<Article>();
            var articleId = new ArticleShortId(NextNumericId());
            article.setup(x => x.ArticleId).Return(new ArticleId());
            article.setup(x => x.ArticleShortId).Return(articleId);
            article.setup(x => x.Owner).Return(owner);
            article.setup(x => x.OwnerId).Return(owner.OwnerId);
            return article;
        }

        public Manager Manager(UserId userId = null)
        {
            userId = userId ?? new UserId();
            return new Manager(userId, "manager@test.phundus.ch", "The Manager");
        }

        public Owner Owner(OwnerType ownerType = OwnerType.Organization)
        {
            return new Owner(new OwnerId(), "The Owner", ownerType);
        }

        public Store Store(OwnerId ownerId = null)
        {
            ownerId = ownerId ?? new OwnerId();
            var result = fake.an<Store>();
            result.setup(x => x.StoreId).Return(new StoreId());
            result.setup(x => x.OwnerId).Return(ownerId);
            result.setup(x => x.Name).Return("The store name");
            return result;
        }
    }
}