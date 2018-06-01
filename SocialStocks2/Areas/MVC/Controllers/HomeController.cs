using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SocialStocks2.Models;
using MongoDB.Driver;

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

            var mongoContext = new mongoCloud();

            var allCustomers = mongoContext.Customers.Find(x => true).ToList();

            //Data d = new Data();

            ViewBag.Customers = allCustomers;
            //d.GetCustomers();

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
            var mongoContext = new mongoCloud();


            //Data d = new Data();

            //d.AddCustomer(CustomerName, CustomerAddress, CustomerPhone);

            var customer = new BCustomer
            {
                NAME = CustomerName,
                ADDRESS = CustomerAddress,
                PHONE = CustomerPhone
            };

            mongoContext.Customers.InsertOne(customer);

            return Json(new { Data = RenderRazorViewToString("Customers",
                mongoContext.Customers.Find(x => true).ToList()) });

        }


    }
}