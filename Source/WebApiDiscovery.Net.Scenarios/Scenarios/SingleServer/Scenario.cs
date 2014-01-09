using System;
using System.Net;
using System.Threading.Tasks;

using NLog;

namespace Krowiorsch.Scenarios.SingleServer
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Task StartScenrio()
        {
            return Task.Factory.StartNew(() =>
            {
                new WebApiServer().StartServer(8069, () => Logger.Debug("Aufruf")); 
                DiscoverableServer.Register("test", new Uri("http://localhost:8069"));
            });
        }
    }
}