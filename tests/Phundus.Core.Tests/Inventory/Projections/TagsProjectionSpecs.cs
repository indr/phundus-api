using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Tests.Inventory.Projections
{
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Articles;
    using Phundus.Inventory.Projections;

    public class tags_projection_concern : inventory_projection_concern<TagsProjection, TagData>
    {
        protected static ArticleId theProductId = new ArticleId();
    }

    [Subject(typeof(TagsProjection))]
    public class tp_when_handling_product_tagged_for_a_new_tag : tags_projection_concern
    {
        private Because of = () =>
            sut.Handle(new ProductTagged(theManager, theProductId, theOwnerId, "tag1"));

        private It should_insert_entity = () =>
            insertedOrUpdated("tag1", x =>
            {
                x.Name.ShouldEqual("tag1");
                x.Count.ShouldEqual(1);
            });
    }

    [Subject(typeof(TagsProjection))]
    public class tp_when_handling_product_tagged_for_an_existing_tag : tags_projection_concern
    {
        private Establish ctx = () =>
            entity.Count = 1;

        private Because of = () =>
            sut.Handle(new ProductTagged(theManager, theProductId, theOwnerId, "tag1"));

        private It should_update_entity = () =>
            insertedOrUpdated("tag1", x =>
            {
                x.Name.ShouldEqual("tag1");
                x.Count.ShouldEqual(2);
            });
    }

    [Subject(typeof (TagsProjection))]
    public class tp_when_handling_product_untagged_for_a_tag_with_count_greater_than_1 : tags_projection_concern
    {
        private Establish ctx = () =>
            entity.Count = 2;

        private Because of = () =>
            sut.Handle(new ProductUntagged(theManager, theProductId, theOwnerId, "tag1"));

        private It should_update_entity = () =>
            updated("tag1", x =>
                x.Count.ShouldEqual(1));
    }

    [Subject(typeof (TagsProjection))]
    public class tp_when_handling_product_untagged_for_a_tag_with_count_equal_1 : tags_projection_concern
    {
        private Establish ctx = () =>
        {
            entity.Name = "tag1";
            entity.Count = 1;
        };

        private Because of = () =>
            sut.Handle(new ProductUntagged(theManager, theProductId, theOwnerId, "tag1"));

        private It should_delete_entity = () =>
            deleted(x =>
                x.Name.ShouldEqual("tag1"));
    }
}
