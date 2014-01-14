using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

using NLog;

namespace Krowiorsch.Scenarios
{
    public class WebApiServer
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        HttpSelfHostServer _server;

        public string BaseAdress { get; private set; }

        public void StartServer(int port, Action onRequest)
        {
            BaseAdress = string.Format("http://{1}:{0}", port, Environment.MachineName);
            var config = new HttpSelfHostConfiguration(BaseAdress);
            config.Routes.MapHttpRoute("All", "{*route}", new { Controller = "Internal", Action = "Default"});

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            _server = new HttpSelfHostServer(config);
            _server.OpenAsync().ContinueWith(t =>
            {
                if(t.IsFaulted)
                {
                    Logger.Warn("Fehler beim hochfahren");
                    return;
                }

                Logger.Debug(string.Format("Start Server on {0}", port));
                
            }).Wait();
        }

        public void StopServer()
        {
            _server.CloseAsync().Wait();
            Logger.Debug(string.Format("Start Server on {0}", BaseAdress));
        }
    }

    public class InternalController : ApiController
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public string Default()
        {
            Logger.Debug("Server: Hello on Port:" + Request.RequestUri.Port);
            return "Hello on Port:" + Request.RequestUri.Port;
        }
    }

}