using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; } 
        IProductRepository Products { get; } 
        ILookupRepository Lookups { get; }
        IInventoryRepository Inventories { get; }
        ISalesorderRepository Saleorders  { get; }
        IPermissionRepository Permissions { get; } 
        bool Commit();
        Task<bool> CommitAsync();
    }
}
