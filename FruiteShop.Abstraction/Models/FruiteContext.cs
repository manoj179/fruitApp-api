using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models
{
    public class FruiteContext :DbContext
    {
        public FruiteContext(DbContextOptions<FruiteContext> options) 
            :base(options){ }


        public DbSet<User> Users { get; set; }

        public DbSet<Fruite> Fruites { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Orders> Orders { get; set; }
    }
}
