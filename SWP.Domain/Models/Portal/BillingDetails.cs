using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.Portal
{
    public class BillingDetails : BaseModel
    {
        public string PhoneNumber { get; set; }

        public string CompanyFullName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Address { get; set; }

        public string AddressCorrespondence { get; set; }

        public string City { get; set; }

        public string Vivodership { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string NIP { get; set; }

        public string REGON { get; set; }

        public string PESEL { get; set; }

        public string KRS { get; set; }
    }
}
