using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class CustomerJob
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool Active { get; set; }

        public int Priority { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        [MaxLength(50)]
        [Required]
        public string UpdatedBy { get; set; }
    }
}
