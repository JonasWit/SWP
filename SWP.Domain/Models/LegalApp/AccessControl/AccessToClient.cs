using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp.AccessControl
{
    public class AccessToClient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ClientId { get; set; }
    }
}
