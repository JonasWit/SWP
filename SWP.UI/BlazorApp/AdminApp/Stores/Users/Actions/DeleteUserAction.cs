using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class DeleteUserAction : IAction
    {
        public const string DeleteUser = "DELETE_USER";
        public string Name => DeleteUser;

        public UserModel Arg { get; set; }
    }
}
