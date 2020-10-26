using SWP.Domain.Models.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SWP.Domain.Models.News
{
    public class NewsRecord : BaseModel //todo: add to context
    {
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Body { get; set; }
        public string Image { get; set; } = "";

        [MaxLength(100)]
        public string Description { get; set; } = "";
        [MaxLength(100)]
        public string Tags { get; set; } = "";
        [MaxLength(100)]
        public string Category { get; set; } = "";
    }
}
