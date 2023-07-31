using DemoWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.EFCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<ClassRoom> ClassRooms { get; set; }

        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
