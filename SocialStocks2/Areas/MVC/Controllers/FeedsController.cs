using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;
using SocialStocks2;
using SocialStocks2.Models;
using System.Xml.Linq;

namespace SocialStocks2.Areas.MVC.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class FeedsController : Controller
    {
        //
        // GET: /Feeds/

        public ActionResult Index()
        {
            return Feeds();
        }

        public ActionResult Feeds()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("FeedsBSKO");
        }

        public ActionResult MyFeeds()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Data d = new Data();
                string u = d.UserIdFromName(User.Identity.Name);

                ViewData.Add("CurrentUser",
                     new WebUser()
                     {
                         UserId = u,
                         Feeds = d.GetUserFeeds(u).ToList<Feed>()
                     }
                      );

                d = null;

                return View("MyFeeds");
            }
            catch (Exception exception)
            {

                return View("Error", 
                    new HandleErrorInfo(exception,"Feeds", "MyFeeds"));
            }

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ListMyFeeds()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new HttpUnauthorizedResult("User Not Logged In"));
            }

            try
            {
                Data d = new Data();
                string u = d.UserIdFromName(User.Identity.Name);

                List<Feed> Feeds = d.GetUserFeeds(u).ToList<Feed>();
                JsonResult results = Json(Feeds, JsonRequestBehavior.AllowGet);
                
                return results;
            }
            catch (Exception exception)
            {
                return Json(exception.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private string RenderRazorViewToString(string viewName, object feeds)
        {
            ViewData.Model = feeds;
            using (var sw = new StringWriter())
            {
                var viewResult =
                  ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext =
                  new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private List<RSSItem> tryCraigs(string linkContent, DateTimeOffset mostRecent)
        {
            List<RSSItem> items = new List<RSSItem>();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(linkContent);

                XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
                
                mgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                mgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
                mgr.AddNamespace("taxo", "http://purl.org/rss/1.0/modules/taxonomy/");
                mgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                mgr.AddNamespace("dcterms", "http://purl.org/dc/terms/");
                mgr.AddNamespace("admin", "http://webns.net/mvcb/");
                mgr.AddNamespace("syn", "http://purl.org/rss/1.0/modules/syndication/");
                mgr.AddNamespace("ev", "http://purl.org/rss/1.0/modules/event/");
                mgr.AddNamespace("a", "http://purl.org/rss/1.0/");
               
                XmlNodeList list = doc.SelectNodes("//a:item", mgr);
                foreach (XmlNode node in list)
                {
                    string dateVal = node.SelectSingleNode("dcterms:issued", mgr).InnerText;
                    DateTime dateTimeVal = Convert.ToDateTime(dateVal);

                    if (dateTimeVal > mostRecent.DateTime)
                    {
                        items.Add(new RSSItem()
                        {
                            Link = node.SelectSingleNode("a:link", mgr).InnerText,
                            Title = node.SelectSingleNode("a:title", mgr).InnerText,
                            PubDate = dateVal,
                            PubDateTime = dateTimeVal,
                            Description = node.SelectSingleNode("a:description", mgr).InnerText,
                            Author = node.SelectSingleNode("dc:source", mgr).InnerText,
                            Id = Guid.NewGuid().ToString(),
                            Background = "#ffffff"
                        });
                    }
                }
            }
            catch { }

            return items;
        }

        private List<RSSItem> tryFeedBurner(string linkContent, DateTimeOffset mostRecent)
        {
            List<RSSItem> items = new List<RSSItem>();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(linkContent);

                XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

                mgr.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
                mgr.AddNamespace("wfw", "http://wellformedweb.org/CommentAPI/");
                mgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                mgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                mgr.AddNamespace("sy", "http://purl.org/rss/1.0/modules/syndication/");
                mgr.AddNamespace("slash", "http://purl.org/rss/1.0/modules/slash/");
                mgr.AddNamespace("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");
                mgr.AddNamespace("feedburner", "http://rssnamespace.org/feedburner/ext/1.0");

                XmlNodeList list = doc.SelectNodes("//item", mgr);
                foreach (XmlNode node in list)
                {
                    string dateVal = node.SelectSingleNode("pubDate", mgr).InnerText;

                    DateTime dateTimeVal;

                    try
                    {
                        dateTimeVal = Convert.ToDateTime(dateVal);
                    }
                    catch
                    {
                        dateTimeVal = DateTime.Now;
                    }

                    if (dateTimeVal > mostRecent.DateTime)
                    {
                        string auth = "";
                        XmlNode x = node.SelectSingleNode("dc:creator", mgr);

                        if (x != null)
                        {
                            auth = x.InnerText;
                        }

                        items.Add(new RSSItem()
                        {
                            Link = node.SelectSingleNode("link", mgr).InnerText,
                            Title = node.SelectSingleNode("title", mgr).InnerText,
                            PubDate = dateVal,
                            PubDateTime = dateTimeVal,
                            Description = node.SelectSingleNode("description", mgr).InnerText,
                            Author = auth,
                            Id = Guid.NewGuid().ToString(),
                            Background = "#ffffff"
                        });
                    }
                }
            }
            catch { }

            return items;
        }

        private List<RSSItem> tryGetTwitter(string linkContent, DateTimeOffset mostRecent)
        {
            List<RSSItem> items = new List<RSSItem>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(linkContent);

                XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

                mgr.AddNamespace("google", "http://base.google.com/ns/1.0");
                mgr.AddNamespace("openSearch", "http://a9.com/-/spec/opensearch/1.1/");
                mgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
                mgr.AddNamespace("twitter", "http://api.twitter.com/");
                mgr.AddNamespace("georss", "http://www.georss.org/georss");

                XmlNodeList list = doc.SelectNodes("//atom:entry", mgr);
                foreach (XmlNode node in list)
                {
                    string dateVal = node.SelectSingleNode("atom:published", mgr).InnerText;
                    DateTime dateTimeVal = Convert.ToDateTime(dateVal);

                    if (dateTimeVal > mostRecent.DateTime)
                    {
                        items.Add(new RSSItem()
                        {
                            Link = node.SelectSingleNode("atom:link", mgr).InnerText,
                            Title = node.SelectSingleNode("atom:title", mgr).InnerText,
                            PubDate = dateVal,
                            PubDateTime = dateTimeVal,
                            Description = node.SelectSingleNode("atom:content", mgr).InnerText,
                            Author = node.SelectSingleNode("atom:author", mgr).InnerText,
                            Id = Guid.NewGuid().ToString(),
                            Background = "#ffffff"
                        });
                    }
                }
            }
            catch { }

            return items;
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ReadFeeds(string Link, string Id)
        {
            string jsonResponse = "";

            FeedSession feedSession;
            DateTimeOffset mostRecent;
            bool firstLoad = false;

            if (Session[Id] == null)
            {
                firstLoad = true;
                feedSession = new FeedSession();
                feedSession.RSSItems = new List<RSSItem>();
                feedSession.LastRead = DateTime.MinValue;
                mostRecent = new DateTimeOffset(DateTime.MinValue);
            }
            else
            {
                feedSession = (FeedSession)Session[Id];
                mostRecent = new DateTimeOffset(feedSession.LastRead);
                foreach (RSSItem i in feedSession.RSSItems)
                {
                    i.Background = "#dddddd";
                }
            }

            try
            {
                var req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(Link);
                req.Method = "GET";
                req.UseDefaultCredentials = true;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                var rep = req.GetResponse();
                XmlReader reader = XmlReader.Create(rep.GetResponseStream());

                // XmlReader reader = XmlReader.Create(Link);
                SyndicationFeed feed = null;
                try
                {
                    feed = SyndicationFeed.Load(reader);                  
                }
                catch (Exception syndException)
                {
                    string rssFeedAsString = "";

                    if (syndException.Message.Contains("The element with name 'html' and namespace '' is not an allowed feed format"))
                    {
                        var webClient = new System.Net.WebClient();
                        // hide ;-)
                        webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        // fetch feed as string
                        var content = webClient.OpenRead(Link);
                        var contentReader = new StreamReader(content);
                        rssFeedAsString = contentReader.ReadToEnd();
                        // convert feed to XML using LINQ to XML and finally create new XmlReader object
                        feed = SyndicationFeed.Load(XDocument.Parse(rssFeedAsString).CreateReader());
                    }

                    if (syndException.Message.Contains("An error was encountered when parsing a DateTime value in the XML"))
                    {
                       // var webClient = new System.Net.WebClient();
                     
                       // webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                       // // fetch feed as string
                       // var content = webClient.OpenRead(Link);
                       // var contentReader = new StreamReader(content);
                       // rssFeedAsString = contentReader.ReadToEnd();
                       // // convert feed to XML using LINQ to XML and finally create new XmlReader object
                       // feed = SyndicationFeed.Load(XDocument.Parse(rssFeedAsString).CreateReader());
                    }
                }

                if (feed==null) { throw new Exception("Feed is NULL.");  }               

                foreach (SyndicationItem item in feed.Items.OrderBy(x=>x.PublishDate))
                {
                    // Ignore the date if the session had not this feed.
                    if (item.PublishDate > mostRecent || firstLoad)
                    {
                        mostRecent = item.PublishDate;

                        feedSession.RSSItems.Add(
                            new RSSItem()
                            {
                                Id = item.Id==null?Guid.NewGuid().ToString():item.Id,
                                Link = item.Links[0].Uri.AbsoluteUri,
                                Description = item.Summary==null?"":item.Summary.Text,
                                Title = item.Title.Text,
                                PubDate = Convert.ToString(item.PublishDate),
                                Author = item.Authors.Count > 0 ? item.Authors[0].Name : "",
                                Background = "#ffffff"
                            });

                        checkSyndicationContentNode(feedSession.RSSItems[feedSession.RSSItems.Count - 1], item);

                        cleanUpScriptsEmbedded(feedSession.RSSItems[feedSession.RSSItems.Count - 1]);

                        // Clean up Denningers Ids
                        string tempId = feedSession.RSSItems[feedSession.RSSItems.Count - 1].Id;
                        tempId = tempId.Replace("/", "").Replace("=", "").Replace(":", "").Replace("?", "");
                        feedSession.RSSItems[feedSession.RSSItems.Count - 1].Id = tempId;
                    }
                }
                reader.Close();
                feed = null;

                feedSession.LastRead = mostRecent.DateTime;
                Session[Id] = feedSession;

                jsonResponse = RenderRazorViewToString("RSSItems", feedSession.RSSItems);
                
            }
            catch (Exception exception)
            {
                jsonResponse = exception.Message;

                try
                {
                    XmlDocument doc = new XmlDocument();

                    doc.Load(Link);

                    List<RSSItem> items = tryGetTwitter(doc.OuterXml, mostRecent);

                    if (items.Count == 0)
                    {
                        items = tryFeedBurner(doc.OuterXml, mostRecent);
                    }

                    if (items.Count == 0)
                    {
                        items = tryCraigs(doc.OuterXml, mostRecent);
                    }

                    if (items.Count > 0)
                    {
                        feedSession.LastRead = items.OrderBy(x => x.PubDateTime).Last<RSSItem>().PubDateTime;
                        feedSession.RSSItems = items;
                        Session[Id] = feedSession;
                        jsonResponse = RenderRazorViewToString("RSSItems", items);
                    }
                } 
                catch 
                { 
                    // smash 
                }
            }

            return Json(
              new
              {
                  Data = jsonResponse
              }, 
              JsonRequestBehavior.AllowGet);
        }

        private void cleanUpScriptsEmbedded(RSSItem item)
        {
            //Clean up scripts in the item
            string desc = item.Description;

            // strip any dangerous code.
            if (desc.Contains("NcodeImageResizer.createOn(this);"))
            {
                desc = desc.Replace("NcodeImageResizer.createOn(this);", "");

                item.Description = desc;
            }
        }

        private void checkSyndicationContentNode(RSSItem rssItem, SyndicationItem item)
        {
            if (item.Content == null)
            {
                try
                {
                    if (item.ElementExtensions.Count > 0)
                    {
                        rssItem.Description =
                        item.ElementExtensions.ReadElementExtensions<string>("encoded",
                        "http://purl.org/rss/1.0/modules/content/")[0];
                    }
                }
                catch { }
            }
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(string id)
        {
            Data d = new Data();

            string u = d.UserIdFromName(User.Identity.Name);

            d.DeleteFeed(id);

            List<Feed> Feeds = d.GetUserFeeds(u).ToList<Feed>();

            d = null;

            return Json(
                new
                {
                    Data = RenderRazorViewToString("FeedList", Feeds)
                });
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(string txtTitle, string txtUrl)
        {
            Data d = new Data();
            string u = d.UserIdFromName(User.Identity.Name);
            d.InsertFeed(u, txtUrl, txtTitle);

            List<Feed> Feeds = d.GetUserFeeds(u).ToList<Feed>();

            d = null;

            return Json(
                new
                {
                    Data = RenderRazorViewToString("FeedList", Feeds)
                });
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Update(string Id, string Title, string Url)
        {
            try
            {
                Data d = new Data();

                d.UpdateFeed(Id, Url, Title);

                d = null;

                Feed updated = new Feed();

                updated.Id = Id;
                updated.Title = Title;
                updated.Url = Url;
                updated.Editing = false;

                return Json(updated);
            }
            catch (Exception ee)
            {
                return Json(ee.Message);
            }
        }


        public ActionResult GetHtmlPage(string path)
        {
            return new FilePathResult(path, "text/html");
        }
    }
}
