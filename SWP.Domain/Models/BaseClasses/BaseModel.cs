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
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
        [MaxLength(50)]
        [Required]
        public string UpdatedBy { get; set; }
        [MaxLength(50)]
        [Required]
        public string CreatedBy { get; set; }
    }
}
