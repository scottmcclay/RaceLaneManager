using System;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;
using System.Web.Http;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(RaceLaneManager.Startup))]

namespace RaceLaneManager
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Debug.WriteLine("Mapping signalr");

            HubConfiguration hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);

            Debug.WriteLine("Mapping /tournament");
            app.Map("/tournament", spa =>
            {
                spa.Use((context, next) =>
                {
                    context.Request.Path = new PathString("/index.html");
                    context.Request.PathBase = new PathString("");

                    return next();
                });

                FileServerOptions o = new FileServerOptions();
                o.RequestPath = PathString.Empty;
                o.FileSystem = new PhysicalFileSystem(@"./website");
                spa.UseFileServer(o);
            });

            Debug.WriteLine("Mapping root");
            FileServerOptions options = new FileServerOptions();
            options.RequestPath = PathString.Empty;
            options.FileSystem = new PhysicalFileSystem(@"./website");
            options.EnableDefaultFiles = true;
            options.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseFileServer(options);

            Debug.WriteLine("Mapping web api");
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }
    }
}
