using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.SharedKernel.Data.Repository;
using DWHDashboard.SharedKernel.Model;

namespace DWHDashboard.ProfileManagement.Infrastructure.Repository
{
    public class TabWorkbookRepository : BaseRepository<TableauWorkbook>,ITabWorkbookRepository
    {
        public TabWorkbookRepository(DwhDashboardContext context) : base(context)
        {
        }

        public async Task<SyncSummary<TableauWorkbook>> AddOrUpdateAsync(IEnumerable<TableauWorkbook> tableauWorkbooks)
        {
            var exisitngWorkbooks = GetAll().ToList();
            
            var summary = TableauWorkbook.GenerateSyncSummary(exisitngWorkbooks, tableauWorkbooks, "Workbooks");

            Create(summary.InsertList);

            await SaveAsync();

            Update(summary.UpdateList);
            Void(summary.VoidsList);

            await SaveAsync();
            
            return summary;
        }
     

        public void Void(IEnumerable<TableauWorkbook> tableauWorkbooks)
        {
            var voidList = tableauWorkbooks.ToList();

            foreach (var t in voidList)
            {
                t.Voided = true;
            }
            Update(voidList);
        }

        public override void Update(TableauWorkbook entity)
        {

            base.Update(entity);
        }
    }
}