using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Models.LegalApp
{
    public class CaseContactPerson : Person
    {
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
