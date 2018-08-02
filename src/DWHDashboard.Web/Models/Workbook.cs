using System.Collections.Generic;
using Newtonsoft.Json;

namespace DWHDashboard.Web.Models
{

    public class Workbook
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContentUrl { get; set; }
        public string SiteId { get; set; }
        public List<View> Views { get; set; }=new List<View>();


        public void AddView(View view)
        {
            view.Workbook = this;
            view.WorkbookId = Id;
            Views.Add(view);
        }
        public void AddViews(List<View> views)
        {
            foreach (var v in views)
            {
                AddView(v);
            }
        }
        public override string ToString()
        {
            return $@"{Name} ({Id})";
        }
    }
}

/*
 <tsResponse>
  <pagination pageNumber="page-number"     pageSize="page-size"     totalAvailable="total-available" />
  <workbooks>
    <workbook 
          id="workbook-id" name="name"          contentUrl="content-url"  
          showTabs="show-tabs-flag"
          size="size-in-megabytes"
          createdAt="datetime-created"
          updatedAt="datetime-updated"  >
     <project id="project-id" name="project-name" />
     <owner id="user-id" />
     <tags>
        <tag label="tag"/>
        ... additional tags ...
     </tags>
   </workbook>
   ... additional workbooks ...
  </workbooks>
</tsResponse>
 */
