using DWHDashboard.SharedKernel;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class ViewConfig:Entity
    {
        public string Section { get; set; }
        public string Display { get; set; }
        public string Containing { get; set; }
        public string NotContaining { get; set; }
        public decimal Rank { get; set; }
    }
}