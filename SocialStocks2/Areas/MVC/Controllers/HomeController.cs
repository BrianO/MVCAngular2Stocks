using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SocialStocks2.Models;
 
namespace SocialStocks2.Areas.MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: MVC/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Page with Bing Maps.";

            Data d = new Data();

            ViewBag.Customers = d.GetCustomers();

            return View();
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

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddCustomer(string CustomerName, string CustomerAddress, string CustomerPhone)
        {
            Data d = new Data();

            d.AddCustomer(CustomerName, CustomerAddress, CustomerPhone);

            return Json(new { Data = RenderRazorViewToString("Customers", d.GetCustomers()) });

        }


    }
}