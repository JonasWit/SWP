using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp.AccessControl
{
    public class AccessToCase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CaseId { get; set; }
    }
}
