using DWHDashboard.ProfileManagement.Core.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.Web.Models
{
    public class OrgAccessViewModel
    {
        public List<Organization> TempOrgs { get; set; }
        public List<TableauView> TableauViews { get; set; }
        public IEnumerable<SelectListItem> TempOrgsList { get; set; }

        public IEnumerable<SelectListItem> GetTempOrgsList(List<Organization> tempOrgs)
        {
            var selectList = new List<SelectListItem>();
            foreach (var element in tempOrgs)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element.Id.ToString(),
                    Text = $"{element.Code} - {element.Name}"
                });
            }

            return selectList;
        }

        public List<TableauView> GetOrgTableauViews(Guid orgId)
        {
            var org = TempOrgs.FirstOrDefault(x => x.Id == orgId);
            var viewIds = org.Views.Select(x => x.TabViewId);

            return TableauViews.Where(x => viewIds.Contains(x.Id)).ToList();
        }
    }
}