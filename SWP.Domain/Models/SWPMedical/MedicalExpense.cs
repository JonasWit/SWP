using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Models.SWPMedical
{
    public class MedicalExpense : BaseModel
    {
        public string Reason { get; set; }
    }
}
