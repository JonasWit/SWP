using SWP.Domain.Models.Licenses;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.General
{
    public class UserData
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(100)]
        [Required]
        public string Comment { get; set; }

        public List<License> Licenses { get; set; }
        public List<LogRecord> LogRecords { get; set; }
    }
}
