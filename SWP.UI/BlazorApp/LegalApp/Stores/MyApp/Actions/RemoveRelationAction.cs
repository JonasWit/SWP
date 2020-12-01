using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.MyApp.Actions
{
    public class RemoveRelationAction : IAction
    {
        public const string RemoveRelation = "REMOVE_RELATION";
        public string Name => RemoveRelation;
    }
}
