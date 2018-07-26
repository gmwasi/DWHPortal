using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWHDashboard.DashboardData.Models
{
    [Table("lkp_USGPartnerMenchanism")]
    public class PartnerMechanism
    {
        [Key]
        public string MFL_Code { get; set; }

        public string FacilityName { get; set; }
        public string County { get; set; }
        public string Agency { get; set; }
        public string MechanismID { get; set; }
        public string Implementing_Mechanism_Name { get; set; }
        public string Mechanism { get; set; }
    }
}