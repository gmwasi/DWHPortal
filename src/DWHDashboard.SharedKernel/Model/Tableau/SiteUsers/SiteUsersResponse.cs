using DWHDashboard.SharedKernel.Model.Tableau.SignIn;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DWHDashboard.SharedKernel.Model.Tableau.SiteUsers
{
    public class SiteUsersResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("users")]
        public Users Users { get; set; }

        public List<User> GetUsers()
        {
            return Users.UserList;
        }

        public User FindUser(string username)
        {
            User found = null;

            var users = GetUsers();

            if (null != users)
                found = users
                    .FirstOrDefault(x => x.Name.ToLower() == username.ToLower());

            return found;
        }
    }
}