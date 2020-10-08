using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.BaseClasses
{
    public class Person : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(50)]
        [Required]
        public string Surname { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        [MaxLength(50)]
        public string AlternativePhoneNumber { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
