using System;
using System.Linq;
using System.Net.Http;

using Krowiorsch.MessageHandlers;

using NLog;

namespace Krowiorsch
{
    class Program
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static HttpClient _client;

        static void Main(string[] args)
        {
            DiscoveryClient.Initialize();

            var discoveryClient = new DiscoveryClient();

            using(var scenrio = new Scenarios.MultipleServer.Scenario())
            {
                var task = scenrio.StartScenrio();
                task.Wait();

                Console.WriteLine("Press Any Key");

                string line;

                while ((line = Console.ReadLine()) != "")
                {
                    if(line.StartsWith("quit"))
                        return;

                    if (line.StartsWith("discoverall"))
                    {
                        DiscoverAll(line.Split(' ').Skip(1).First(), discoveryClient);
                        continue;
                    }

                    if (line.StartsWith("discover"))
                    {
                        Discover(line.Split(' ').Skip(1).First(), discoveryClient);
                        continue;
                    }

                    if(line.StartsWith("requeston"))
                    {
                        RequestOn(line.Split(' ').Skip(1).First(), discoveryClient);
                        continue;
                    }

                    Logger.Info("unknown command");
                }
            }

            
        }

        static void RequestOn(string serviceUri, IDiscoveryClient discoveryClient, int requestCount = 10)
        {
            var httpClient = new HttpClient(new PerRequestMessagehandler(new Uri(serviceUri), discoveryClient))
            {
                BaseAddress = discoveryClient.Discover(new Uri(serviceUri))
            };

            for(int i = 0; i < requestCount; i++)
            {
                var result = httpClient.GetStringAsync("Hello").Result;
                Logger.Info(string.Format("Result:{0}", result));
            }
        }

        static void Discover(string name, DiscoveryClient client)
        {
            var service = client.DiscoverByServiceIdentifier(name);

            Logger.Info("Found Service on {0} for {1}", service, name);
        }

        static void DiscoverAll(string name, DiscoveryClient client)
        {
            var services = client.DiscoverAllByServiceIdentifier(name);

            foreach(var service in services)
            {
                Logger.Info("Found Service on {0} for {1}", service, name);
            }
        }
    }
}
