using System;
using System.Linq;

using NLog;

namespace Krowiorsch
{
    class Program
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
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

                    Logger.Info("unknown command");
                }
            }

            
        }

        static void Discover(string name, DiscoveryClient client)
        {
            var service = client.Discover(name);

            Logger.Info("Found Service on {0} for {1}", service, name);
        }

        static void DiscoverAll(string name, DiscoveryClient client)
        {
            var services = client.DiscoverAll(name);

            foreach(var service in services)
            {
                Logger.Info("Found Service on {0} for {1}", service, name);
            }
        }
    }
}
