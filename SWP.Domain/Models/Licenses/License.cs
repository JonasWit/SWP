using SWP.Domain.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.Licenses
{
    public class License
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string AppName { get; set; }

        [MaxLength(100)]
        [Required]
        public string LicenseName { get; set; }

        public DateTime Expires { get; set; }


        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        [MaxLength(50)]
        public string UpdatedBy { get; set; }


        public int UserDataId { get; set; }

        public UserData UserData { get; set; }
    }
}
