using Data.Identity;
using StockApp.Data;
using StockApp.Data.Enitities;

namespace DAL.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        void AddLocation(InventoryLocation location, ApplicationUser user = null);
    }
}
