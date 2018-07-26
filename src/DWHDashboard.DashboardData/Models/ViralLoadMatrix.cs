using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWHDashboard.DashboardData.Models
{
    [Table("Fact_ViralLoadMatrix")]
    public class ViralLoadMatrix
    {
        [Key]
        [Column("PatientPK")]
        public string PatientPk { get; set; }

        [Column("NUM")]
        public long Num { get; set; }

        [Column("PatientId")]
        public string PatientId { get; set; }

        public string SiteCode { get; set; }

        [Column("VisitID")]
        public string VisitId { get; set; }

        public DateTime OrderedByDate { get; set; }
        public DateTime ? ResultDate { get; set; }
        public string TestName { get; set; }
        public string TestResult { get; set; }
        public string Emr { get; set; }
        public string Project { get; set; }

        [Column("MonthsAfterARTStart")]
        public  int ? MonthsAfterArtStart { get; set; }

        [Column("StartARTDate")]
        public DateTime ? StartArtDate { get; set; }

        public string StartRegimen { get; set; }
    }
}