using System;
using System.Dynamic;
using DWHDashboard.SharedKernel;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class OrganisationAccess:Entity
    {
        public Guid OrganisationId { get; set; }
        public Guid TabViewId { get; set; }

        public OrganisationAccess()
        {
        }

        public OrganisationAccess(Guid organisationId, Guid tabViewId)
        {
            OrganisationId = organisationId;
            TabViewId = tabViewId;
        }
    }
}