using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWHDashboard.DashboardData.Models
{
    [Table("DATIM_FACT_NEWLY_ENROLLED_BASELINECD4")]
    public class DatimNewlyEnrolledBaselineCd4
    {
        [Key]
        [Column("PatientPK")]
        public string PatientPk { get; set; }

        public string Gender { get; set; }
        public int AgeLastVisit { get; set; }
        public int AgeAtArtStart { get; set; }
        public string AgeGroup { get; set; }

        [Column("StartARTDate")]
        public DateTime StartArtDate { get; set; }

        [Column("LastARTDate")]
        public DateTime LastArtDate { get; set; }

        public string FacilityName { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string ImplementingMechnanism { get; set; }
        public string Agency { get; set; }
        public string SiteCode { get; set; }

        [Column("bCD4")]
        public float Bcd4 { get; set; }

        [Column("bCD4Date")]
        public DateTime Bcd4Date { get; set; }
    }
}