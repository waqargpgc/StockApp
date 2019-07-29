using StockApp.Data.Enitities;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> GetProductsForList();
        Product GetProductDetail(int id);
    }
}
 