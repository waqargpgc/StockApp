using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using StockApp.Data.Enitities;
using StockApp.Data;

namespace DAL.Repositories
{
    public class SalesorderRepository : Repository<Order>, ISalesorderRepository
    {
        private AppDbContext _context;

        public SalesorderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void AddOrderItem(OrderDetail item)
        {
            _context.OrderDetails.Add(item);
        }

        public Order GetOrderInfo(int id)
        {
            var order = (
                from o in _context.Orders
                join orItms in _context.OrderDetails on o.OrderId equals orItms.OrderId
                join p in _context.Products on orItms.ProductId equals p.ProductId
                where o.OrderId == id
                select new
                {
                    Order = o,
                    Items = orItms,
                    Product = p
                }
             ).FirstOrDefault();

            return _context
                     .Orders
                     .Include(o => o.OrderDetailList)
                     .FirstOrDefault(x => x.OrderId == id);
        }

    }
}
