using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Models.Portal
{
    public class UserLicense : BaseModel
    {
        [MaxLength(50)]
        public string Product { get; set; }
        public DateTime ValidTo { get; set; }


        //todo: remove this relation
        public int BillingDetailId { get; set; }
        public BillingDetail BillingDetail { get; set; }
    }
}
