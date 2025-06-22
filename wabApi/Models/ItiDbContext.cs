using Microsoft.EntityFrameworkCore;

namespace wabApi.Models
{
    public class ItiDbContext:DbContext
    {
        public ItiDbContext()
        {

        }
        public ItiDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }

    }
}
