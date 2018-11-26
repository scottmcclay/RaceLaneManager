using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.ComponentModel;
using RaceLaneManager;

namespace RaceLaneManagerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(url: "http://+:8000/"))
            {
                Console.WriteLine("Race Lane Manager is available at http://localhost:8000");
                Console.WriteLine("Press any key to exit.");
                Console.Read();
            }
        }
    }
}
