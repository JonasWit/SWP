using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.SWPMedical
{
    public class Appointment : BaseModel
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Message { get; set; }

        [MaxLength(1000)]
        public string MedicalNotes { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
