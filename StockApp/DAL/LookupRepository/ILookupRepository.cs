using StockApp.Data.Enitities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface ILookupRepository
    {
        IEnumerable<InventoryLocation> InventoryLocationList();
        IEnumerable<Supplier> SupplierList();
        IEnumerable<ProductCategory> ProductCategoryList();
        IEnumerable<Manufacturer> ManufacturerList();
    }
}
 