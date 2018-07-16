using System.Collections.Generic;
using DWHDashboard.SharedKernel;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class Impersonator:Entity
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDisabled { get; set; }
        public virtual ICollection<User> Users { get; set; }=new List<User>();

        public Impersonator()
        {
        }

        public Impersonator(string userName, string userId, string password, bool isDefault)
        {
            UserName = userName;
            UserId = userId;
            Password = password;
            IsDefault = isDefault;
        }

        public void AddUser(User user)
        {
            user.ImpersonatorId = Id;
            Users.Add(user);
        }
    }
}