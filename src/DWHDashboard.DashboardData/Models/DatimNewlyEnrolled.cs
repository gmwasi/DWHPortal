using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWHDashboard.DashboardData.Models
{
    [Table("DATIM_FACT_NEWLY_ENROLLED")]
    public class DatimNewlyEnrolled
    {
        [Key]
        public int Id { get; set; }

        public string Gender { get; set; }
        public string AgeGroup { get; set; }

        [Column("StartARTDateEOM")]
        public DateTime StartArtDateEom { get; set; }

        public string FacilityName { get; set; }
        public string County { get; set; }
        public string SubCounty { get; set; }
        public string ImplementingMechnanism { get; set; }
        public string Agency { get; set; }
        public string SiteCode { get; set; }
        public int Total { get; set; }
        public int Year { get; set; }
        public string Quarter { get; set; }

        [Column("Care &Treatment Partner")]
        public string CareAndTreatmentPartner { get; set; }

        [Column("Month_Year")]
        public string MonthYear { get; set; }

        [Column("Start ART Date")]
        public DateTime StartArtDate { get; set; }
    }
}