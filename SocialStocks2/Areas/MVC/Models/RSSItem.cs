using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SocialStocks2.Models
{
    [Serializable()]
    public class RSSItem : object
    {
        public string Title { get; set; }

        public string Link { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public string PubDate { get; set; }

        public string Author { get; set; }

        public string Id { get; set; }

        public string Background { get; set; }

        public DateTime PubDateTime { get; set; }
    }
}