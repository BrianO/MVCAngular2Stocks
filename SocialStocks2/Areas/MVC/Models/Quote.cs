using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialStocks2.Models
{
    [Serializable()]
    public class Quote
    {
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }

        [Display(Name = "Ask")]
        public string Ask { get; set; }

        [Display(Name = "Bid")]
        public string Bid { get; set; }

        [Display(Name = "Last Trade")]
        public string LastTrade { get; set; }

        [Display(Name = "Ex Dividend")]
        public string ExDividend { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Yield")]
        public string DividendYield { get; set; }

        [Display(Name = "PE")]
        public string PERatio { get; set; }

        [Display(Name = "PercentChange")]
        public string PercentChange { get; set; }

    }
}