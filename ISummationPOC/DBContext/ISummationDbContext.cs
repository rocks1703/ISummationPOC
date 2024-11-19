using ISummationPOC.Entity;
using Microsoft.EntityFrameworkCore;
namespace ISummationPOC.DBContext
{
    public class ISummationDbContext : DbContext
    {
        public ISummationDbContext()
        {

        }

        public ISummationDbContext(DbContextOptions<ISummationDbContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        public DbSet<UserTypes> userTypes { get; set; }

    }

}
