using System;
using DWHDashboard.ProfileManagement.Core.Model;

namespace DWHDashboard.Web.Models
{
    public class OrgUserViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int IsSteward { get; set; }

        public DWHDashboard.ProfileManagement.Core.Model.User GetUser()
        {
            var user = new DWHDashboard.ProfileManagement.Core.Model.User();
            user.Id = Id.ToString();
            user.FullName = FullName;
            user.PhoneNumber = PhoneNumber;
            return user;
        }
    }
}