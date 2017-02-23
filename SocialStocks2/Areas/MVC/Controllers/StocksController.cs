using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using SocialStocks2.Models;

namespace SocialStocks2.Areas.MVC.Controllers
{
    public class StocksController : Controller
    {
        // GET: MVC/Stocks
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult StocksAngular()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("StocksAngular");
        }

        [Authorize()]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult StocksJSON()
        {
            try
            {
                Data d = new Data();
                string u = d.UserIdFromName(User.Identity.Name);

                List<Stock> outList = d.GetUserStocks(u).ToList<Stock>();

                if (outList.Count == 0)
                {
                    outList = d.GetUserStocks(
                        d.UserIdFromName("brian.f.oneil@gmail.com")
                        ).ToList<Stock>();
                }

                return Json(outList, JsonRequestBehavior.AllowGet);

                d = null;
            }
            catch (Exception exception)
            {

                return Json(exception.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ReadNewsData(string Link)
        {
            List<RSSItem> RSSItems = new List<RSSItem>();

            try
            {
                getNewsData(Link, RSSItems);

            }
            catch (System.Exception exception)
            {
                return Json(new
                {
                    Data = "Error " + exception
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Data = RSSItems
            }, JsonRequestBehavior.AllowGet);
        }


        private void getNewsData(string Link, List<RSSItem> RSSItems)
        {
            XmlReader reader = XmlReader.Create(Link);

            SyndicationFeed feed = SyndicationFeed.Load(reader);
            foreach (SyndicationItem item in feed.Items
                .OrderByDescending(x => x.PublishDate)
                .Take<SyndicationItem>(7))
            {
                RSSItems.Add(
                    new RSSItem()
                    {
                        Id = item.Id == null ? Guid.NewGuid().ToString() : item.Id,
                        Link = item.Links[0].Uri.AbsoluteUri,
                        Description = item.Summary.Text,
                        Title = item.Title.Text,
                        PubDate = Convert.ToString(item.PublishDate),
                        Author = item.Authors.Count > 0 ? item.Authors[0].Name : "",
                        Background = "#ffffff"
                    });

                // checkSyndicationContentNode(RSSItems[RSSItems.Count - 1], item);
                // cleanUpScriptsEmbedded(RSSItems[RSSItems.Count - 1]);
            }

            reader.Close();
            feed = null;
        }




        private Quote getQuoteFromYahoo(string symbol)
        {
            string jsonResponse = "";

            string s =
                "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20%28%22" +
                symbol +
                "%22%29&env=store://datatables.org/alltableswithkeys";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(s));

            WebResponse response = req.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = reader.ReadToEnd();
            }

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(jsonResponse);

            Quote q = new Quote();
            q.Name = xdoc.SelectSingleNode("//Name").InnerText;
            q.LastTrade = xdoc.SelectSingleNode("//LastTradePriceOnly").InnerText;
            q.Ask = xdoc.SelectSingleNode("//Ask").InnerText;
            q.Bid = xdoc.SelectSingleNode("//Bid").InnerText;
            q.DividendYield = xdoc.SelectSingleNode("//DividendYield").InnerText;
            q.ExDividend = xdoc.SelectSingleNode("//ExDividendDate").InnerText;
            q.PERatio = xdoc.SelectSingleNode("//PERatio").InnerText;
            q.PercentChange = xdoc.SelectSingleNode("//PercentChange").InnerText;
            q.Symbol = symbol;

            if (q.PERatio.Length == 0)
            {
                q.PERatio = "n/a";
            }

            if (q.ExDividend.Length == 0)
            {
                q.ExDividend = "n/a";
            }

            return q;
        }


        [Authorize()]
        [AcceptVerbs(HttpVerbs.Get)]
        [OutputCache(Duration = 360, Location = System.Web.UI.OutputCacheLocation.Server)]
        public JsonResult ReadSymbol(string Id)
        {
            try
            {
                Quote q = getQuoteFromYahoo(Id);

                return Json(
                  new
                  {
                      Data = q.Name
                  },
                  JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(
                  new
                  {
                      Data = string.Empty
                  },
                  JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize()]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ReadStockQuote(string Id)
        {
            Quote q;

            if (Id.Length > 0)
            {
                q = getQuoteFromYahoo(Id);
            }
            else
            {
                q = new Quote();
            }

            return Json(q, JsonRequestBehavior.AllowGet);
        }


        [Authorize()]
        [AcceptVerbs(HttpVerbs.Get)]
        [OutputCache(Duration = 360, VaryByParam = "Id")]
        public JsonResult ReadPrice(string Id)
        {
            string lastTrade = string.Empty;
            string color = string.Empty;
            string error = string.Empty;

            try
            {
                Quote q = getQuoteFromYahoo(Id);

                lastTrade = "$" + q.LastTrade;
                if (q.PercentChange.Contains("-"))
                {
                    color = "Red";
                }
                else
                {
                    color = "Green";
                }

            }
            catch (System.Exception exception)
            {
                lastTrade = "error";
                error = exception.Message;
            }

            return Json(
             new Stock()
             {
                 Price = lastTrade,
                 Color = color,
 //                Message = error,
                 Symbol = Id
             },
             JsonRequestBehavior.AllowGet);
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Remove(Stock item)
        {
            Data d = new Data();

            if (item.Id == null && item.Symbol != null)
            {

                string u = d.UserIdFromName(User.Identity.Name);

                List<Stock> stocks = d.GetUserStocks(u).ToList<Stock>();

                item.Id = stocks.First(x => x.Symbol == item.Symbol).Id;
            }

            d.DeleteStock(item.Id);

            d = null;

            return Json(
                new
                {
                    Data = "Deleted"
                });
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddJSON(string Symbol)
        {
            if (Symbol.Length == 0)
            {
                return Json("Empty Param.");
            }

            Data d = new Data();

            string u = d.UserIdFromName(User.Identity.Name);

            d.InsertStock(u, Symbol);

            d = null;

            return Json("Success");
                
        }


    }

}