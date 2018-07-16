using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DWHDashboard.SharedKernel;
using DWHDashboard.SharedKernel.Model;
using DWHDashboard.SharedKernel.Model.Tableau;
using DWHDashboard.SharedKernel.Utility;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class TableauView:TableauEntity
    {
        public string WorkbookTableauId { get; set; }
        public string CustomParentName { get; set; }
        [NotMapped]
        public string CustomName { get; set; }

        public decimal? Rank { get; set; } = 100000m;

        [NotMapped]
        public virtual string VoidedInfo
        {
            get { return Voided ? "Yes" : "No"; }
        }
        [NotMapped]
        public virtual bool CanView { get; set; }

        [NotMapped]
        public virtual string CanViewInfo
        {
            get { return CanView ? "checked" : ""; }
        }

        public Guid TableauWorkbookId { get; set; }

        public virtual ICollection<OrganisationAccess> TempOrgs { get; set; } = new List<OrganisationAccess>();

        public TableauView()
        {
            
        }

        public static TableauView CreateModified(TableauView view,string customParentName)
        {
            view.CustomParentName= customParentName;
            return view;
        }


        public TableauView(string tableauId, string name, string workbookTableauId)
        {
            TableauId = tableauId;
            Name = name;
            WorkbookTableauId = workbookTableauId;            
        }

        public static List<TableauView> Generate(List<View> views, string workbookId)
        {
            var tabViews = new List<TableauView>();
            foreach (var view in views)
            {
                tabViews.Add(new TableauView(view.Id, view.Name,workbookId));
            }
            return tabViews;
        }

        public static List<TableauView> GenerateShowingChecked(List<TableauView> allViews, List<TableauView> orgViews)
        {
            if (orgViews.Count > 0)
            {
                var checkedIds = orgViews.Select(x => x.Id);

                var checkedViews = allViews.Where(x => checkedIds.Contains(x.Id));
                foreach (var v in checkedViews)
                {
                    v.CanView = true;
                }
            }
            return allViews;
        }

        public override string ToString()
        {
            return $@"{Name} ({TableauId})";
        }

    }
}