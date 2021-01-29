using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Domain.Model.Users.Entities;

namespace Office4U.Data.Ef.SqlServer.Contexts
{
    public class DataContext :
       IdentityDbContext<
           AppUser,
           AppRole,
           int,
           IdentityUserClaim<int>,
           AppUserRole,
           IdentityUserLogin<int>,
           IdentityRoleClaim<int>,
           IdentityUserToken<int>
       >
    {
        public DataContext() { }
        public DataContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Article> Articles { get; set; }
        public DbSet<ArticlePhoto> ArticlePhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ar => ar.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();
        }


        // used for migrations
        // TODO: read project configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=Office4U.Article;Trusted_Connection=True;");
        }
    }
}
