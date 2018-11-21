using API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Entities
{
    public class UserDbContext : IdentityDbContext<UserModel>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserModel>(user => user.HasIndex(x => x.Locale).IsUnique(false));
            builder.Entity<Organization>(org => {
                org.ToTable("Organizations");
                org.HasKey(x => x.Id);
                org.HasMany<UserModel>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            });

        }
    }
}