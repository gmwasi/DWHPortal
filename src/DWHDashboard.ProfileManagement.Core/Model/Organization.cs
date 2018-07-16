using DWHDashboard.SharedKernel;
using System;
using System.Collections.Generic;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class Organization : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<OrganisationAccess> Views { get; set; } = new List<OrganisationAccess>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        public void UpdateViews(List<Guid> tabViewIds,List<Guid> charts)
        {
            Views = new List<OrganisationAccess>();

            //addNew
            foreach (var tabViewId in tabViewIds)
            {
                Views.Add(new OrganisationAccess(Id, tabViewId));
            }

            //addNew
            foreach (var tabViewId in charts)
            {
                Views.Add(new OrganisationAccess(Id, tabViewId));
            }
        }
    }
}