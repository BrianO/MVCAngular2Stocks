using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialStocks2.Models
{
    [Serializable()]
    public class WebUser
    {
        public WebUser()
        {
            Feeds = new List<Feed>();
            Stocks = new List<Stock>();
        }

        public string UserId { get; set; }

        public List<Feed> Feeds;
        public List<Stock> Stocks;
    }
}