namespace Phundus.Inventory.Queries
{
    using Common;
    using Common.Domain.Model;
    using Projections;

    public interface IStoresQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        StoresRow GetByOwnerId(OwnerId ownerId);

        StoresRow FindByOwnerId(OwnerId ownerId);
    }
}