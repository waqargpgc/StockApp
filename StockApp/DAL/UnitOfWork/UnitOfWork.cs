using StockApp.Data;
using StockApp.Data.Enitities;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private ILookupRepository _lookups;
        private ICustomerRepository _customers;
        private IProductRepository _products;
        private IInventoryRepository _inventory;
        private SalesorderRepository _saleorders; 
        private IPermissionRepository _permission; 

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers
        {
            get
            {
                if (_customers == null)
                    _customers = new CustomerRepositoy(_context);

                return _customers;
            }
        }
        public IProductRepository Products
        {
            get
            {
                if (_products == null)
                    _products = new ProductRepository(_context);

                return _products;
            }
        }

        public IInventoryRepository Inventories
        {
            get
            {
                if (_inventory == null)
                    _inventory = new InventoryRepository(_context);

                return _inventory;
            }
        }

        public ISalesorderRepository Saleorders  
        {
            get
            {
                if (_saleorders == null)
                    _saleorders = new SalesorderRepository(_context);

                return _saleorders;
            }
        }

        public ILookupRepository Lookups
        {
            get
            {
                if (_lookups == null)
                    _lookups = new LookupRepository(_context);

                return _lookups;
            }
        }
        public IPermissionRepository Permissions 
        {
            get
            {
                if (_permission == null)
                    _permission = new PermissionRepository(_context);

                return _permission; 
            }
        }


        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
