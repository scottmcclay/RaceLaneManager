using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace Rlm.Service
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private IContainer _components = null;
        private ServiceProcessInstaller _serviceProcessInstaller;
        private ServiceInstaller _serviceInstaller;

        public ProjectInstaller()
        {
            _serviceProcessInstaller = new ServiceProcessInstaller();
            _serviceProcessInstaller.Account = ServiceAccount.LocalService;
            _serviceProcessInstaller.Password = null;
            _serviceProcessInstaller.Username = null;

            _serviceInstaller = new ServiceInstaller();
            _serviceInstaller.ServiceName = "RaceLaneManager";
            _serviceInstaller.AfterInstall += new InstallEventHandler(this.ProjectInstallerAfterInstall);

            this.Installers.AddRange(new Installer[] { _serviceProcessInstaller, _serviceInstaller });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_components != null))
            {
                _components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ProjectInstallerAfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
