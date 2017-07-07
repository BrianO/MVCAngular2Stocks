using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialStocks2.Models
{
    [Serializable]
    public class FeedSession
    {
        public DateTime LastRead { get; set; }
        public List<RSSItem> RSSItems { get; set; }

    }
}