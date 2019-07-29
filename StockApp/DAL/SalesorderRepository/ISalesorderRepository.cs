using StockApp.Data;
using StockApp.Data.Enitities;

namespace DAL.Repositories 
{
    public interface ISalesorderRepository : IRepository<Order>
    {
        void AddOrderItem(OrderDetail item);
        Order GetOrderInfo(int id);
        //IEnumerable<CustomerModel> GetAllIncludingChildren();
    }
}
 