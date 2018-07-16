using DWHDashboard.SharedKernel.Utility;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.SharedKernel.Model
{
    public class TableauEntity : Entity
    {
        public virtual string TableauId { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Voided { get; set; }

        public static SyncSummary<T> GenerateSyncSummary<T>(IEnumerable<T> existingTableauViews, IEnumerable<T> newTableauViews, string name) where T : TableauEntity
        {
            var summary = new SyncSummary<T>(name);
            //new
            var newViews = newTableauViews.SniffOutNew(existingTableauViews, new TableauEntityComparer<T>()).ToList();
            summary.AddInsertList(newViews);
            //updated
            var updatedViews = existingTableauViews.SniffOutUpdated(newTableauViews, new TableauEntityComparer<T>()).ToList();
            var updates = updatedViews;
            //CopyId(existingTableauViews, updates);
            summary.AddUpdateList(updates);

            //voided
            var voidedViews = existingTableauViews.SniffOutVoided(newTableauViews, new TableauEntityComparer<T>()).ToList();
            var voids = voidedViews;
            //CopyId(existingTableauViews, voids);
            summary.AddVoidList(voids);
            return summary;
        }

        public static void CopyId<T>(IEnumerable<T> existingTableauViews, IEnumerable<T> newTableauViews) where T : TableauEntity
        {
            foreach (var e in existingTableauViews)
            {
                newTableauViews.FirstOrDefault(x => x.TableauId == e.TableauId).Id = e.Id;
            }
        }
    }
}