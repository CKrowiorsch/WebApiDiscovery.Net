using System;
using System.Net;
using System.Threading.Tasks;

using NLog;

namespace Krowiorsch.Scenarios.SingleServer
{
    public class Scenario : IDisposable
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        static Random rnd = new Random();

        public Task StartScenrio()
        {
            return Task.Factory.StartNew(() =>
            {
                //new WebApiServer().StartServer(8069, () => Logger.Debug("Aufruf")); 
                DiscoverableServer.Register("test" + rnd.Next(1, 10000), new Uri(string.Format("http://{0}:8069", Environment.MachineName)));
            });
        }

        public void Dispose()
        {

        }
    }
}