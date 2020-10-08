using SWP.Domain.Models.BaseClasses;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class Note : BaseModel
    {
        public bool Active { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
        public int Priority { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
