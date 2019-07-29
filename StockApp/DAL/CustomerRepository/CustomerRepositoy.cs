using System;
using System.Linq;
using StockApp.Data;
using StockApp.Data.Enitities;
using StockApp.Models.Customers;

namespace DAL.Repositories
{
    public class CustomerRepositoy : Repository<Customer>, ICustomerRepository
    {
        private AppDbContext _context;

        public CustomerRepositoy(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAllCustomers(CustomerResourceParameters parameters)
        {
            var customers = FindAll(c => c.SharedKey == parameters.Key && c.IsActive && !c.IsDeleted); 

            #region Filter, Search & sort
            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                var name = parameters.Name.Trim().ToLowerInvariant();
                customers = customers.Where(a => a.Name.ToLowerInvariant().Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Email))
            {
                var email = parameters.Email.Trim().ToLowerInvariant();
                customers = customers.Where(a => a.Email.ToLowerInvariant().Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Mobile))
            {
                var mobile = parameters.Mobile.Trim().ToLowerInvariant();
                customers = customers.Where(a => a.Mobile.ToLowerInvariant().Contains(mobile));
            }

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                var searchQueryForWhereClause = parameters.SearchQuery.Trim().ToLowerInvariant();

                customers = customers
                    .Where(a => a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Address.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Telephone.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Email.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Mobile.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            #endregion

            return customers;
        }

    }
}
