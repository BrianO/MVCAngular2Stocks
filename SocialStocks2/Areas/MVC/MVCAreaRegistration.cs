using System.Web.Mvc;

namespace SocialStocks2.Areas.MVC
{
    public class MVCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MVC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //"MVC/{controller}/{action}/{id}",

            context.MapRoute(
                "MVC_default",
                "{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}