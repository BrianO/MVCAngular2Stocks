using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialStocks2.Models
{
    public class Feed
    {
        public Feed()
        {
            Editing = false;
        }

        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "URL")]
        public string Url { get; set; }

        [Display(Name = "Editing")]
        public bool Editing { get; set; }

    }
}