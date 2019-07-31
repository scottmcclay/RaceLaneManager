using System.ServiceProcess;

namespace Rlm.Service
{
    public class Program
    {
        static public void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new RaceLaneManagerService()
            };

            ServiceBase.Run(servicesToRun);

            //StartOptions options = new StartOptions();
            //string url = "http://localhost:5000";
            //options.Urls.Add(url);
            ////options.Urls.Add("http://raspberrypi:5000");
            ////options.Urls.Add("http://127.0.0.1:5000");
            ////options.Urls.Add("http://localhost:80");

            //using (WebApp.Start<Startup>(options))
            //{
            //    Console.WriteLine("Hosting site at {0}", url);
            //    Console.WriteLine("Press enter to quit.");
            //    Console.ReadKey();
            //}
        }
    }
}
