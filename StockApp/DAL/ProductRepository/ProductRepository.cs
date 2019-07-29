using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using StockApp.Data.Enitities;
using StockApp.Data;

namespace DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Product> GetProductsForList()
        {
            // var employees = model.Employees.Where(e => e.Tags.Any(t => tagsIDList.Contains(t.TagID))); 
            // var employees = (from e in model.Employees from t in e.Tags where tagsIDList.Contains(t.TagID) select e);
        
            return _context.Products
                       .Include(p => p.ProductCategory) 
                      .Include(x => x.Inventories).ToList();
              
        }

        public Product GetProductDetail(int id)
        {
            return _context.Products
                       .Include(p => p.ProductCategory)
                      .Include(x => x.Inventories)
                      .FirstOrDefault(p => p.ProductId == id);

        }

    }
}
