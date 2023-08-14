using DemoWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.EFCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<CompanyPostHashtag> CompanyPostHashtags { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<BranchJob> BranchJobs { get; set; }
        public DbSet<BranchJobCompany> BranchJobCompanies { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
