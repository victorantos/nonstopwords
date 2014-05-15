using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace wwwroot
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
         name: "Games",
         url: "games/{action}/{id}",
         defaults: new { controller = "Game", action = "Index", id = RouteParameter.Optional }
     );
            routes.MapRoute(
                 name: "Scripts",
                 url: "Scripts/wordsearch_data/{id}",
                 defaults: new { controller = "Game", action = "GetWordsearchHTML", id = RouteParameter.Optional }
             );
            routes.MapRoute(
               name: "Game",
               url: "game/{id}",
               defaults: new { controller = "Game", action = "Index" }
           );
        }
    }
}
