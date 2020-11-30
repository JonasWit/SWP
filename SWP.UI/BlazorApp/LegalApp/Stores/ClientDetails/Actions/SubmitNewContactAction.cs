using SWP.Application.LegalSwp.ContactPeopleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp.Stores.ClientDetails.Actions
{
    public class SubmitNewContactAction : IAction
    {
        public const string SubmitNewContact = "SUBMIT_NEW_CONTACT";
        public string Name => SubmitNewContact;

        public CreateContactPerson.Request Arg { get; set; }
    }
}
