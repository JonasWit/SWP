using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Domain.Models.SWPMedical
{
    public class MedicalFinance : BaseModel
    {
        //link to patient

        public List<MedicalExpense> Expenses { get; set; }
        public List<MedicalIncome> Incomes { get; set; }







    }
}
