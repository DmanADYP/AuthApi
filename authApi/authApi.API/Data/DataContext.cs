using authApi.API.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace authApi.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Value> Values {get;set;}
        public DbSet<User> Users { get; set; }
    }

}