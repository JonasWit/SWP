using SWP.Domain.Models.BaseClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.SWPLegal
{
    public class ClientJob : BaseModel
    {
        public bool Active { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int Priority { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
