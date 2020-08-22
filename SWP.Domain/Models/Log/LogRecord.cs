using SWP.Domain.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.Log
{
    public class LogRecord
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Message { get; set; }

        public int UserDataId { get; set; }
        public UserData UserData { get; set; }

        public DateTime Created { get; set; }
    }
}
