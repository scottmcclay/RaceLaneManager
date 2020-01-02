using Microsoft.Owin.Hosting;
using System;
using System.ComponentModel;
using System.ServiceProcess;
using Rlm.Web;

namespace Rlm.Service
{
    public class RaceLaneManagerService : ServiceBase
    {
        private IContainer _components = null;
        public string BaseAddress
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    return "http://+:80";
                }

                return "http://+:8000";
            }
        }

        private IDisposable _server = null;

        public RaceLaneManagerService()
        {
            _components = new Container();
            this.ServiceName = "RaceLaneManager";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            _server = WebApp.Start<Startup>(url: this.BaseAddress);
        }

        protected override void OnStop()
        {
            if (_server != null)
            {
                _server.Dispose();
            }

            base.OnStop();
        }
    }
}
