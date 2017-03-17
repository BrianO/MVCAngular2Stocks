using System.Web.Mvc;
using System.Web.Routing;

namespace SocialStocks2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "ngOverride",
              url: "Stocks/StocksAngular/{*.}",
              defaults: new { controller = "Stocks", action = "StocksAngular" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).DataTokens = new RouteValueDictionary(new { area = "MVC" });
        }
    }
}