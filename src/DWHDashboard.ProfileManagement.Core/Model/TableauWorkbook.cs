using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using DWHDashboard.SharedKernel;
using DWHDashboard.SharedKernel.Model;
using DWHDashboard.SharedKernel.Model.Tableau;

namespace DWHDashboard.ProfileManagement.Core.Model
{

    public class TableauWorkbook: TableauEntity
    {
        public virtual ICollection<TableauView> TabViews { get; set; }=new List<TableauView>();

        public TableauWorkbook()
        {
        }

        public TableauWorkbook(string tableauId, string name)
        {
            TableauId = tableauId;
            Name = name;
        }

        public static List<TableauWorkbook> Generate(List<Workbook> workbooks)
        {
            var tabWorkbooks=new List<TableauWorkbook>();
            foreach (var workbook in workbooks)
            {
                tabWorkbooks.Add(new TableauWorkbook(workbook.Id,workbook.Name));
            }
            return tabWorkbooks;
        }
        public void AddTabView(TableauView tabView)
        {
            tabView.TableauWorkbookId = Id;
            tabView.WorkbookTableauId = TableauId;
            TabViews.Add(tabView);
        }

        public void AddTabViews(List<TableauView> tabViews)
        {
            foreach (var tabView in tabViews)
            {
                AddTabView(tabView);
            }
        }
        
        public override string ToString()
        {
            return $@"{Name}";
        }
    }
}

