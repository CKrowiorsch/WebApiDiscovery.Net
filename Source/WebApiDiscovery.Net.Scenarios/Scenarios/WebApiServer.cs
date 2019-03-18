using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NLog;
using Owin;

namespace Krowiorsch.Scenarios
{
    public class WebApiServer
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        IDisposable _apiSubscription;

        public string BaseAdress { get; private set; }

        public void StartServer(int port, Action onRequest)
        {
            BaseAdress = string.Format("http://{1}:{0}", port, Environment.MachineName);
            _apiSubscription = WebApp.Start(
                new StartOptions { ServerFactory = "Nowin", Port = port },
                app => Startup.Configuration(app));
            
        }

        public void StopServer()
        {
            _apiSubscription?.Dispose();
            Logger.Debug(string.Format("Start Server on {0}", BaseAdress));
        }


        static class Startup
        {
            public static void Configuration(IAppBuilder appBuilder)
            {
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();

                config.Formatters.Clear();
                config.Formatters.Add(new JsonMediaTypeFormatter { SerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects } });

                config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;


                config.EnsureInitialized();

                appBuilder.UseWebApi(config);
            }
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