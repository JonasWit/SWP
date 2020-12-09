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
        public string UserId { get; set; }
        [MaxLength(100)]
        public string Application { get; set; }
        [MaxLength(100)]
        public string Type { get; set; }
        public int RelatedUsers { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
