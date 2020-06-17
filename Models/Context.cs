using Microsoft.EntityFrameworkCore;

namespace Login.Models
{
    public class Context : DbContext 
    {
        public Context(DbContextOptions options) : base(options){}

        public DbSet<User> Users {get;set;}
    }
}