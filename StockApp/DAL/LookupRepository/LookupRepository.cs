using StockApp.Data;
using StockApp.Data.Enitities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class LookupRepository : ILookupRepository
    {
        private AppDbContext _context;

        public LookupRepository(AppDbContext context)
        {
            _context = context;
        }

        //public IEnumerable<object> GetEnquiredByList()
        //{
        //    var data = new List<object>
        //    {
        //        new {Source  = "Father"},
        //        new {Source  = "Mother"},
        //        new {Source  = "Guardian"},
        //        new {Source  = "Self"},
        //    };
        //    return data;

        //    //  return new List<object> [{ "Father", "Mother", "Guardian", "Self" }];
        //}

        public IEnumerable<InventoryLocation> InventoryLocationList()
        {
            return _context.InventoryLocations.ToList();
        }

        public IEnumerable<Supplier> SupplierList()
        {
            return _context.Suppliers.Where(s => s.IsActive && (!s.IsDeleted)).ToList();
        }

        public IEnumerable<ProductCategory> ProductCategoryList()
        {
            return _context.ProductCategories.Where(c => c.IsActive && (!c.IsDeleted)).ToList();
        }

        public IEnumerable<Manufacturer> ManufacturerList()
        {
            return _context.Manufacturers.Where(m => m.IsActive && (!m.IsDeleted)).ToList();
        }
    }
}
