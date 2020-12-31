using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp.AccessControl
{
    public class AccessToPanel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PanelId { get; set; }
    }
}
