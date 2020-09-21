using SWP.Domain.Models.BaseClasses;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class CashMovement : BaseModel
    {
        public double Amount { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}
