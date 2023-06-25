using Microsoft.EntityFrameworkCore;
using SimpleAuth.Entities;

namespace SimpleAuth.Models
{
    public class UserContext : DbContext
    {
        public DbSet<UserInfo> usersdb { get; set; }   
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
