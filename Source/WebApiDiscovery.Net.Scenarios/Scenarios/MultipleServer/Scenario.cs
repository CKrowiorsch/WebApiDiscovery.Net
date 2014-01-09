using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using NLog;

namespace Krowiorsch.Scenarios.MultipleServer
{
    public class Scenario : IDisposable
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly List<WebApiServer> _servers = new List<WebApiServer>();

        public Task StartScenrio()
        {
            return Task.Factory.StartNew(() =>
            {
                StartAndRegisterServer(IpPort.GetNextPortAvailable(10000));
                StartAndRegisterServer(IpPort.GetNextPortAvailable(10000));
            });
        }

        void StartAndRegisterServer(int port)
        {
            var server = new WebApiServer();
            server.StartServer(port, () => Logger.Debug("Aufruf"));
            DiscoverableServer.Register("test", new Uri(string.Format("http://{1}:{0}", port, Environment.MachineName)));

            _servers.Add(server);
        }

        void StopAndUnRegisterServers()
        {
            foreach(var webApiServer in _servers)
            {
                DiscoverableServer.UnRegister("test", new Uri(webApiServer.BaseAdress));
                webApiServer.StopServer();
            }
        }

        public void Dispose()
        {
            StopAndUnRegisterServers();
        }
    }
}