using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.BaseClasses
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        [MaxLength(50)]
        [Required]
        public string UpdatedBy { get; set; }
    }
}
