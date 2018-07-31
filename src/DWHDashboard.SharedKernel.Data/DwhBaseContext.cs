using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DWHDashboard.ProfileManagement.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.SharedKernel.Data
{
    public abstract class DwhBaseContext : IdentityDbContext<User>
    {
        
        protected DwhBaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<IdentityUser>().ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<User>().ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityRole>().ToTable("OrganizationAccess", "dbo");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserOrganizationAccess", "dbo");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", "dbo");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserToken");

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(150));
        }
    }
}