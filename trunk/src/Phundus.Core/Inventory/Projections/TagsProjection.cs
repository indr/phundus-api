namespace Phundus.Inventory.Projections
{
    using System;
    using Common.Eventing;
    using Common.Projecting;
    using Model.Articles;

    public class TagsProjection : ProjectionBase<TagData>,
        ISubscribeTo<ProductTagged>,
        ISubscribeTo<ProductUntagged>
    {
        public void Handle(ProductTagged e)
        {
            InsertOrUpdate(e.TagName, x =>
            {
                x.Count++;
                x.Name = e.TagName;
            });
        }

        public void Handle(ProductUntagged e)
        {
            var entity = Session.Get<TagData>(e.TagName);
            if (entity == null)
                return;

            if (entity.Count == 1)
                Session.Delete(entity);
            else
            {
                entity.Count--;
                Session.Update(entity);
            }
        }
    }

    public class TagData
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}