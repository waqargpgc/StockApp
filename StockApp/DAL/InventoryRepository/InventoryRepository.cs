using Data.Identity;
using StockApp.Data;
using StockApp.Data.Enitities;
using System;

namespace DAL.Repositories
{
    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        private AppDbContext _context;
        public InventoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void AddLocation(InventoryLocation location, ApplicationUser user = null)
        {
            try
            {
                if (user != null)
                {
                    location.CreatedBy = user.Id;
                    location.SharedKey = user.SharedKey;
                }

                if (location != null)
                {
                    location.IsActive = true;
                    location.IsDeleted = false;
                    location.CreatedDate = DateTime.Now;

                    _context.InventoryLocations.Add(location);
                }
            }

            catch (Exception ex) { }
        }
    }
}
