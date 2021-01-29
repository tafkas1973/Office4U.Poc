using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Office4U.Domain.Model.Entities.Articles;
using Office4U.Domain.Model.Entities.Users;

namespace Office4U.Data.Ef.SqlServer.Contexts
{
    public class ReadOnlyDataContext :
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
        public ReadOnlyDataContext() { }
        public ReadOnlyDataContext(DbContextOptions<ReadOnlyDataContext> options) : base(options) { }

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
    }
}
