using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using DWHDashboard.ProfileManagement.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.SharedKernel.Data
{
    public abstract class DwhBaseContext : IdentityDbContext<User>
    {
        
        protected DwhBaseContext(DbContextOptions nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(150));
            modelBuilder.Entity<User>().ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUser>().ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityRole>().ToTable("OrganizationAccess", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserOrganizationAccess", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");
        }*/
    }
}