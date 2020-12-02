using SWP.Domain.Models.BaseClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp
{
    public class TimeRecord : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Lawyer { get; set; }
        [Required]
        public double Rate { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public DateTime EventDate { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
