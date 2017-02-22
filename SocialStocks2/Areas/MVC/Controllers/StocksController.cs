using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

                if (outList.Count==0)
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


    }

}