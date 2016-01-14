namespace Phundus.Inventory.Articles.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ArticleDeleted : DomainEvent
    {
    }
}