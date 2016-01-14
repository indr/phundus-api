namespace Phundus.Inventory.Queries
{
    using Common;
    using Common.Domain.Model;

    public interface IStoreQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        StoreDto GetByOwnerId(OwnerId ownerId);

        StoreDto FindByOwnerId(OwnerId ownerId);
    }
}