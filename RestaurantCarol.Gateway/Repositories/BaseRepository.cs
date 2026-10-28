using RestaurantCarol.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantCarol.Gateway.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly RestaurantCarolDbContext _context;

        protected BaseRepository(RestaurantCarolDbContext context)
        {
            _context = context;
        }
    }
}
