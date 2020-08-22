using Microsoft.AspNetCore.Identity;
using SWP.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.Components.AdminBlazorComponents.ViewModels
{
    public class AdminUsersComponentViewModel
    {
        public List<string> Claims => Enum.GetNames(typeof(ApplicationType)).ToList();
        public List<string> Roles => Enum.GetNames(typeof(RoleType)).ToList();

        public bool Loading { get; set; }

        public UserModel SelectedUser { get; set; }
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        public string ErrorMessage { get; set; } = "";

        public class UserModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }

            public int UserRoleInt 
            {
                get
                {
                    return (int)UserRole;
                }
                set
                {
                    UserRoleInt = value;
                    UserRole = (RoleType)UserRoleInt;
                }
            }

            public RoleType UserRole { get; set; } = RoleType.Users;
            public List<Claim> Claims { get; set; } = new List<Claim>();
        }
    }
}
