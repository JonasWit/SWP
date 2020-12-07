using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Users.Actions
{
    public class DeleteClaimRowAction : IAction
    {
        public const string DeleteClaimRow = "DELETE_CLAIM_ROW";
        public string Name => DeleteClaimRow;

        public Claim Arg { get; set; }
    }
}
