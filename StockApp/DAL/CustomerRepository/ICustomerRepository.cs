using StockApp.Data;
using StockApp.Data.Enitities;
using StockApp.Models.Customers;
using System.Linq;

namespace DAL.Repositories 
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IQueryable<Customer> GetAllCustomers(CustomerResourceParameters parameters);
    }
}
