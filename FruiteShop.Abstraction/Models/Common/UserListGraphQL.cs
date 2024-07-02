using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models.Common
{
    public class GenerictGraphQL
    {

        private readonly FruiteContext _dbContext;

        public GenerictGraphQL(FruiteContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public async Task<List<Orders>> GetOrdersList()
        {
            var data = await _dbContext.Orders.Include(m => m.Fruite).AsNoTracking().ToListAsync();
            return data;
        }

        //public async Task<List<Orders>> GetOrdersList([ScopedService] FruiteContext dbContext) => await dbContext.Orders.AsNoTracking().ToListAsync();

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public async Task<List<User>> GetUsersList()
        {
            var data = await _dbContext.Users.AsNoTracking().ToListAsync();
            return data;
        }


    }
}
