using System.Web.Http;

namespace Rlm.Web
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "RaceLaneManagerApi",
                routeTemplate: "api/tournament/{tournamentId}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { tournamentId = @"\d+" });

            config.Routes.MapHttpRoute(
                name: "RaceLaneManagerTournamentApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            //config.Routes.MapHttpRoute(
            //    name: "RaceLaneManagerEventActionApi",
            //    routeTemplate:  "api/{controller}/{action}/{id}",
            //    defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional });
        }
    }
}
