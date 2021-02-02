using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Domain.Model.Users.Entities;

namespace Office4U.Data.Ef.SqlServer.Contexts
{
    public class QueryDbContext :
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
        public QueryDbContext() { }
        public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options) { }

        // TODO: expose IQueryable here instead of DbSet ?
        public DbSet<Article> Articles { get; set; }
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
