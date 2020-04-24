using Microsoft.EntityFrameworkCore;

namespace MyBackend.EF
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BloggingEFTest;Trusted_Connection=True;MultipleActiveResultSets=true;");
    }
}