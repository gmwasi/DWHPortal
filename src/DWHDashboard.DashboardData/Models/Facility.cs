namespace DWHDashboard.DashboardData.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DimFacilities")]
    public class Facility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FacilityCode { get; set; }

        [StringLength(255)]
        public string FacilityName { get; set; }

        [StringLength(255)]
        public string Province { get; set; }

        [StringLength(255)]
        public string County { get; set; }

        [StringLength(255)]
        public string District { get; set; }

        [StringLength(255)]
        public string Division { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Owner { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(255)]
        public string SubLocation { get; set; }

        [StringLength(255)]
        public string DescriptionofLocation { get; set; }

        [StringLength(255)]
        public string Constituency { get; set; }

        [StringLength(255)]
        public string NearestTown { get; set; }

        public double? Beds { get; set; }

        public double? Cots { get; set; }

        [StringLength(255)]
        public string OfficialLandline { get; set; }

        [StringLength(255)]
        public string OfficialFax { get; set; }

        [StringLength(255)]
        public string OfficialMobile { get; set; }

        [StringLength(255)]
        public string OfficialEmail { get; set; }

        [StringLength(255)]
        public string OfficialAddress { get; set; }

        [StringLength(255)]
        public string OfficialAlternateNo { get; set; }

        [StringLength(255)]
        public string Town { get; set; }

        [StringLength(255)]
        public string PostCode { get; set; }

        [StringLength(255)]
        public string InCharge { get; set; }

        [StringLength(255)]
        public string JobTitleofInCharge { get; set; }

        [StringLength(255)]
        public string Open24Hours { get; set; }

        [StringLength(255)]
        public string OpenWeekends { get; set; }

        [StringLength(255)]
        public string OperationalStatus { get; set; }

        [StringLength(255)]
        public string ANC { get; set; }

        [StringLength(255)]
        public string ART { get; set; }

        [StringLength(255)]
        public string BEOC { get; set; }

        [StringLength(255)]
        public string BLOOD { get; set; }

        [StringLength(255)]
        public string CAES_SEC { get; set; }

        [StringLength(255)]
        public string CEOC { get; set; }

        [Column("C-IMCI")]
        [StringLength(255)]
        public string C_IMCI { get; set; }

        [StringLength(255)]
        public string EPI { get; set; }

        [StringLength(255)]
        public string FP { get; set; }

        [StringLength(255)]
        public string GROWM { get; set; }

        [StringLength(255)]
        public string HBC { get; set; }

        [StringLength(255)]
        public string HCT { get; set; }

        [StringLength(255)]
        public string IPD { get; set; }

        [StringLength(255)]
        public string OPD { get; set; }

        [StringLength(255)]
        public string OUTREACH { get; set; }

        [StringLength(255)]
        public string PMTCT { get; set; }

        [Column("RAD/XRAY")]
        [StringLength(255)]
        public string RAD_XRAY { get; set; }

        [Column("RHTC/RHDC")]
        [StringLength(255)]
        public string RHTC_RHDC { get; set; }

        [Column("TB DIAG")]
        [StringLength(255)]
        public string TB_DIAG { get; set; }

        [StringLength(255)]
        public string TBLABS { get; set; }

        [StringLength(255)]
        public string TBTREAT { get; set; }

        [StringLength(255)]
        public string YOUTH { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [StringLength(255)]
        public string KEPH_Level { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateImported { get; set; }
    }
}