using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Model;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface ILookupService
    {
        Task<List<Organization>> GetOrganizations();
    }
}
