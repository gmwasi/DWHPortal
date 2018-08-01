using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DWHDashboard.ProfileManagement.Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.SharedKernel.Data
{
    public abstract class DwhBaseContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        
        protected DwhBaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoleClaim>(builder =>
            {
                builder.HasOne(roleClaim => roleClaim.Role).WithMany(role => role.Claims).HasForeignKey(roleClaim => roleClaim.RoleId);
                builder.ToTable("RoleClaim");
            });
            modelBuilder.Entity<Role>(builder =>
            {
                builder.ToTable("Role");
            });
            modelBuilder.Entity<UserClaim>(builder =>
            {
                builder.HasOne(userClaim => userClaim.User).WithMany(user => user.Claims).HasForeignKey(userClaim => userClaim.UserId);
                builder.ToTable("UserClaim");
            });
            modelBuilder.Entity<UserLogin>(builder =>
            {
                builder.HasOne(userLogin => userLogin.User).WithMany(user => user.Logins).HasForeignKey(userLogin => userLogin.UserId);
                builder.ToTable("UserLogin");
            });
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("User");
            });
            modelBuilder.Entity<UserRole>(builder =>
            {
                builder.HasOne(userRole => userRole.Role).WithMany(role => role.Users).HasForeignKey(userRole => userRole.RoleId);
                builder.HasOne(userRole => userRole.User).WithMany(user => user.Roles).HasForeignKey(userRole => userRole.UserId);
                builder.ToTable("UserRole");
            });
            modelBuilder.Entity<UserToken>(builder =>
            {
                builder.HasOne(userToken => userToken.User).WithMany(user => user.UserTokens).HasForeignKey(userToken => userToken.UserId);
                builder.ToTable("UserToken");
            });
        }

        public virtual void EnsureSeeded()
        {

        }
    }
}