using System.Collections.Generic;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.SharedKernel.Interfaces;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.ProfileManagement.Core.Interfaces
{
    public interface ITabWorkbookRepository : IRepository<TableauWorkbook>
    {
        Task<SyncSummary<TableauWorkbook>> AddOrUpdateAsync(IEnumerable<TableauWorkbook> tableauWorkbooks);
        void Void(IEnumerable<TableauWorkbook> tableauWorkbooks);
    }
}