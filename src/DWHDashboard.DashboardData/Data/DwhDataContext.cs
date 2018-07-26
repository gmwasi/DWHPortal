using DWHDashboard.DashboardData.Models;
using Microsoft.EntityFrameworkCore;

namespace DWHDashboard.DashboardData.Data
{
    public class DwhDataContext : DbContext
    {
        public DwhDataContext(DbContextOptions<DwhDataContext> options) : base(options)
        {
        }
        public DbSet<DatimNewlyEnrolled> DatimNewlyEnrolleds { get; set; }
        public DbSet<DatimNewlyEnrolledBaselineCd4> DatimNewlyEnrolledBaselineCd4s { get; set; }
        public DbSet<ViralLoadMatrix> ViralLoadMatrices { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<PartnerMechanism> PartnerMechanisms { get; set; }
    }
}