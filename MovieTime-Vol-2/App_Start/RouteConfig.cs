using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MovieTime_Vol_2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Movie", action = "Index", id = 1 }
            );

            routes.MapRoute(
                name: "Watchlist",
                url: "{controller}/{action}/{id}/{watchlistname}",
                defaults: new { controller = "Movie", action = "AddToWatchList", id = "", watchlistname = "" }
                );

            routes.MapRoute(
              name: "Remove",
              url: "{controller}/{action}/{id}/{watchlistname}/{release_date}",
              defaults: new { controller = "Movie", action = "RemoveMovieFromYourList", id = "", watchlistname = "" , release_date= ""}
              );
        }
    }
}
