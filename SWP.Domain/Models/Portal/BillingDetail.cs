using Microsoft.AspNetCore.Identity;
using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.Portal
{
    public class BillingDetail : BaseModel
    {
        public string UserId { get; set; }
        [PersonalData]
        public string PhoneNumber { get; set; }
        [PersonalData]
        public string CompanyFullName { get; set; }
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string Surname { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string AddressCorrespondence { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string Vivodership { get; set; }
        [PersonalData]
        public string Country { get; set; }
        [PersonalData]
        public string PostCode { get; set; }
        [PersonalData]
        public string NIP { get; set; }
        [PersonalData]
        public string REGON { get; set; }
        [PersonalData]
        public string KRS { get; set; }
    }
}
