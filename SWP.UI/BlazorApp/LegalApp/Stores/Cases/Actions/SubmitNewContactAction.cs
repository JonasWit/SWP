using SWP.Application.LegalSwp.ContactPeopleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Cases.Actions
{
    public class SubmitNewContactAction : IAction
    {
        public const string SubmitNewContactRow = "SUBMIT_NEW_CONTACT";
        public string Name => SubmitNewContactRow;

        public CreateContactPerson.Request Request { get; set; }
    }
}
