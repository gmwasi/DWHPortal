using DWHDashboard.SharedKernel.Model.Tableau.SignIn;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DWHDashboard.SharedKernel.Model.Tableau.SiteUsers
{
    public class Users
    {
        [JsonProperty("user")]
        public List<User> UserList { get; set; }
    }
}