using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.DAL
{
    public class DBContext : IdentityDbContext<User,Role,int>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DBContext(DbContextOptions<DBContext> contextOptions) : base(contextOptions) =>
            Database.Migrate();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new User());
            builder.ApplyConfiguration(new Post());
            builder.ApplyConfiguration(new Tag());
            builder.ApplyConfiguration(new Role());
            builder.ApplyConfiguration(new Comment());
        }
/*        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=192.168.6.17\;Database=helloappdb;User ID=sa;Password=e11kpp!!;TrustServerCertificate=True;");
            base.OnConfiguring(optionsBuilder);
        }*/
    }
}