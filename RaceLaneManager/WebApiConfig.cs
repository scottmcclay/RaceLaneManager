using System.Web.Http;

namespace RaceLaneManager
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "RaceLaneManagerApi",
                routeTemplate: "api/Events/{eventId}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
                name: "RaceLaneManagerEventApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional },
                constraints: new { id = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "RaceLaneManagerEventActionApi",
                routeTemplate:  "api/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional });
        }
    }
}
