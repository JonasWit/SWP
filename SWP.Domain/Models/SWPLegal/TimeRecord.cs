using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.SWPLegal
{
    public class TimeRecord : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public TimeSpan RecordedTime { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
