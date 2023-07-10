
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PocketPlanner.Models;

namespace PocketPlanner.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options) 
        {
        
        }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Budget> Budget => Set<Budget>();
    }
}
