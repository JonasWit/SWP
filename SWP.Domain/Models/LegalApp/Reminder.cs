using SWP.Domain.Models.BaseClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp
{
    public class Reminder : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
        public int Priority { get; set; }
        public bool IsDeadline { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
