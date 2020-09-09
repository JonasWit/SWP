using SWP.Domain.Models.BaseClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class Note : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Message { get; set; }

        public bool Active { get; set; }

        public int Priority { get; set; }

        public int CaseId { get; set; }

        public Case Case { get; set; }
    }
}
