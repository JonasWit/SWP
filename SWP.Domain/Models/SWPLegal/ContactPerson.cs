using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Models.SWPLegal
{
    public class ContactPerson : Person
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
