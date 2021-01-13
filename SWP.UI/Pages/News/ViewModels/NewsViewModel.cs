using Microsoft.AspNetCore.Http;
using SWP.Domain.Models.News;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace SWP.UI.Pages.News.ViewModels
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Max 200 Chars.")]
        public string Title { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "Max 1000 Chars.")]
        public string Body { get; set; }

        public string ImagePath { get; set; }

        public string ShortNews => new string($"{Description.Take(10).ToArray()}...");

        [Required]
        [StringLength(100, ErrorMessage = "Max 100 Chars.")]
        public string Description { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Max 100 Chars.")]
        public string Tags { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Max 100 Chars.")]
        public string Category { get; set; }

        //public IFormFile Image { get; set; } = null;
        //public Stream ImageStream { get; set; } = null;
        public string ImageName { get; set; } = null;
        public byte[] ImageStream { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public List<string> Categories { get; set; }

        public NewsViewModel()
        {
            Categories = new List<string>();

            foreach (NewsCategory category in (NewsCategory[])Enum.GetValues(typeof(NewsCategory)))
            {
                Categories.Add(category.ToString());
            }

            Category = Categories.First();
        }

        public static implicit operator NewsViewModel(NewsRecord input) =>
            new NewsViewModel
            {

            };
    }
}
