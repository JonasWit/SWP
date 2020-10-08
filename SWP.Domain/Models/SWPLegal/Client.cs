using SWP.Domain.Models.BaseClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class Client : BaseModel
    {
        public bool Active { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        [Required]
        public string ProfileClaim { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public List<TimeRecord> TimeRecords { get; set; }
        public List<Case> Cases { get; set; }
        public List<ClientJob> Jobs { get; set; }
        public List<CashMovement> CashMovements { get; set; }
    }
}
