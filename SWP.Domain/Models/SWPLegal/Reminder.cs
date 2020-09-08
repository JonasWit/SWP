using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Message { get; set; }

        public bool Active { get; set; }

        public int Priority { get; set; }

        public bool IsDeadline { get; set; }

        public int CaseId { get; set; }

        public Case Case { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        [MaxLength(50)]
        [Required]
        public string UpdatedBy { get; set; }
    }
}
