using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.SWPMedical
{
    public class Patient : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Surname { get; set; }

        [MaxLength(50)]
        [Required]
        public string PhoneNumber { get; set; }

        //[MaxLength(50)]
        //[Required]
        //public string AlternativePhoneNumber { get; set; }

        //[MaxLength(300)]
        //[Required]
        //public string Address { get; set; }

        //[MaxLength(300)]
        //[Required]
        //public string AddressCorrespondence { get; set; }

        [MaxLength(50)]
        [Required]
        public int PESEL { get; set; }

        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public List<Appointment> Appointments { get; set; }

        public int AppointmentId { get; set; }
    }
}
