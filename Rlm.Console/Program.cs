using Microsoft.Owin.Hosting;
using RaceLaneManager;

namespace Rlm.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(url: "http://+:8000/"))
            {
                System.Console.WriteLine("Race Lane Manager is available at http://localhost:8000");
                System.Console.WriteLine("Press any key to exit.");
                System.Console.Read();
            }
        }
    }
}
