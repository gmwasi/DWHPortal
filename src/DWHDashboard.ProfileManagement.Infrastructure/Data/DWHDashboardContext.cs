using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Data;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.ProfileManagement.Infrastructure.Data
{
    public class DwhDashboardContext : DwhBaseContext
    {
        public DwhDashboardContext(DbContextOptions<DwhDashboardContext> options) : base(options)
        {
        }

        public DwhDashboardContext() : base(new DbContextOptions<DwhBaseContext>())
        {
        }

        public static DwhDashboardContext Create()
        {
            return new DwhDashboardContext();
        }

        public DbSet<Impersonator> Impersonators { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<TableauWorkbook> TableauWorkbooks { get; set; }
        public DbSet<TableauView> TableauViews { get; set; }

        public DbSet<ViewConfig> ViewConfigs { get; set; }
        public DbSet<OrganisationAccess> OrganisationAccesses { get; set; }

        public DbSet<UserPreference> UserPreferences { get; set; }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TableauWorkbook>()
                .HasMany(c => c.TabViews)
                .WithRequired()
                .HasForeignKey(f => new { f.TableauWorkbookId });

            modelBuilder.Entity<Organization>()
                .HasMany(c => c.Views)
                .WithRequired()
                .HasForeignKey(f => new { f.OrganisationId });

            modelBuilder.Entity<TableauView>()
                .HasMany(c => c.TempOrgs)
                .WithRequired()
                .HasForeignKey(f => new { f.TabViewId });
        }*/
    }
}