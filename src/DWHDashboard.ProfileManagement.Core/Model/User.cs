﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DWHDashboard.ProfileManagement.Core.Model
{
    public class User : IdentityUser<string>
    {
        public Title Title { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string ReasonForAccessing { get; set; }
        public UserType UserType { get; set; }
        public bool IsTableau { get; set; }
        public string Password { get; set; }
        public bool IsDisabled { get; set; }
        public UserConfirmation UserConfirmed { get; set; }
        public Guid? ImpersonatorId { get; set; }
        public virtual Impersonator Impersonator { get; set; }
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        #region Relationships
        public virtual ICollection<UserToken> UserTokens { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }

        public virtual ICollection<UserLogin> Logins { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }

        #endregion
        
        [NotMapped]
        public string AuthUserName { get; set; }

        [NotMapped]
        public string AuthUserId { get; set; }

        [NotMapped]
        public string AuthToken { get; set; }

        [NotMapped]
        public string AuthSiteId { get; set; }

        public User()
        {
        }

        private User(string userName, string fullName, bool isTableau, string email, string password)
        {
            UserName = userName;
            FullName = fullName;
            IsTableau = isTableau;
            Email = email;
            Password = password;
        }

        public static User CreateTableauUser(SharedKernel.Model.Tableau.SignIn.User signInUser, string password)
        {
            return new User(signInUser.Name, signInUser.FullName, true, signInUser.Email, password);
        }

        public override string ToString()
        {
            return $"{UserName} ({FullName}|{Email}|{AuthUserName})";
        }

        public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateAsync(this);
            // Add custom user claims here
            return userIdentity;
        }

        public void ChangeProfileTo(User updatedUser)
        {
            FullName = updatedUser.FullName;
            PhoneNumber = updatedUser.PhoneNumber;
        }

        public string GetStatus()
        {
            if (
                EmailConfirmed && 
                !IsTableau && 
                UserConfirmed != UserConfirmation.Confirmed &&
                UserType==UserType.Guest
                )
            {
                if(UserName.ToLower()!= "guest@thepalladiumgroup.com".ToLower())
                    return $"You account has limited Access, Please contact your Steward for assistance !";
            }
            return null;
        }

        public void SetId(string id)
        {
            Id = id;
        }
    }

    public class Role : IdentityRole<string>
    {
        public Role() : base()
        {

        }
        public Role(string roleName) : this()
        {
            Name = roleName;
        }

        public virtual ICollection<UserRole> Users { get; set; }
        public virtual ICollection<RoleClaim> Claims { get; set; }
    }

    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }

    public class UserClaim : IdentityUserClaim<string>
    {
        public virtual User User { get; set; }
    }

    public class UserLogin : IdentityUserLogin<string>
    {
        public virtual User User { get; set; }
    }

    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class UserToken : IdentityUserToken<string>
    {
        public virtual User User { get; set; }
    }

    public enum UserType
    {
        None,
        Admin,
        Steward,
        Normal,
        Guest
    }

    public enum Title
    {
        Mr = 1,
        Mrs,
        Ms,
        Dr,
        Prof,
        Eng
    }

    public enum UserConfirmation
    {
        Pending,
        Confirmed,
        Denyed
    }
}