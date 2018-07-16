using System;
using System.Collections.Generic;
using DWHDashboard.SharedKernel;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class TempOrg:Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<OrganisationAccess> Views { get; set; }=new List<OrganisationAccess>();


        public void UpdateViews(List<Guid> tabViewIds)
        {
            Views =new List<OrganisationAccess>();

            //addNew
            foreach (var tabViewId in tabViewIds)
            {
                Views.Add(new OrganisationAccess(Id,tabViewId));
            }
        }
    }
}