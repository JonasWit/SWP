using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.Log
{
    public class LogRecord
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }
}
