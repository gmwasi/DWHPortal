using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CsvHelper.Configuration;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Data;
using EFCore.Seeder.Configuration;
using EFCore.Seeder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TableauWorkbook>()
                .HasMany(c => c.TabViews)
                .WithOne()
                .IsRequired()
                .HasForeignKey(f => new { f.TableauWorkbookId });

            modelBuilder.Entity<Organization>()
                .HasMany(c => c.Views)
                .WithOne()
                .IsRequired()
                .HasForeignKey(f => new { f.OrganisationId });

            modelBuilder.Entity<TableauView>()
                .HasMany(c => c.TempOrgs)
                .WithOne()
                .IsRequired()
                .HasForeignKey(f => new { f.TabViewId });
        }

        public override void EnsureSeeded()
        {
            var csvConfig = new CsvConfiguration
            {
                Delimiter = "|",
                SkipEmptyRecords = true,
                TrimFields = true,
                TrimHeaders = true,
                WillThrowOnMissingField = false
            };

            SeederConfiguration.ResetConfiguration(csvConfig, null, typeof(DwhDashboardContext).GetTypeInfo().Assembly);

            Impersonators.SeedDbSetIfEmpty($"{nameof(Impersonator)}");
            Organizations.SeedDbSetIfEmpty($"{nameof(Organization)}");
            Users.SeedDbSetIfEmpty($"{nameof(User)}");
            ViewConfigs.SeedDbSetIfEmpty($"{nameof(ViewConfig)}");
            SaveChanges();
        }
    }
}