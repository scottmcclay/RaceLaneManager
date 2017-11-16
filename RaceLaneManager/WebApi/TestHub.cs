using System;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace RaceLaneManager.WebApi
{
    public class TestHub: Hub
    {
        public void Test(string message)
        {
            Console.WriteLine("Message from client: {0}", message);
            Clients.All.displayMessage(message);
        }
    }
}
