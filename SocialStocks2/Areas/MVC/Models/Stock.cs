using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialStocks2.Models
{
    [Serializable()]
    public class Stock
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Symbol")]
        public string Symbol { get;  set; }

        [Display(Name = "Price")]
        public string Price { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }
    }
}