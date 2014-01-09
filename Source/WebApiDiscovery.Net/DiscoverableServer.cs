using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Krowiorsch.Impl;
using Krowiorsch.Messages;
using Krowiorsch.Model;

using Newtonsoft.Json;

using NLog;

namespace Krowiorsch
{
    public static class DiscoverableServer
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        static readonly List<Tuple<string, Uri>> ServerUriPairs = new List<Tuple<string, Uri>>();
        static Task _discoveryTask;

        static readonly MulticastClient MulticastClient = MulticastClientFactory.CreateForServiceDiscovery();

        static readonly object SyncLock = new object();

        static DiscoverableServer()
        {
            _discoveryTask = Task.Factory.StartNew(PublishState);
        }

        static void PublishState()
        {
            while (true)
            {
                Thread.Sleep(3000);
                lock (SyncLock)
                {
                    foreach (var pair in ServerUriPairs)
                    {
                        var message = Message.FromObject(new RegisterEndpointMessage(new ServiceEndpoint(pair.Item1, pair.Item2)));
                        MulticastClient.Send(message.ToJson());
                    }
                }
            }
        }

        static public void Register(string serviceName, Uri serviceUri)
        {
            lock (SyncLock)
                ServerUriPairs.Add(new Tuple<string, Uri>(serviceName, serviceUri));
        }

        static public void UnRegister(string serviceName, Uri serviceUri)
        {
            lock (SyncLock)
            {
                ServerUriPairs.RemoveAll(t => t.Item1.Equals(serviceName, StringComparison.OrdinalIgnoreCase) && t.Item2.Equals(serviceUri));

                var message = Message.FromObject(new UnregisterEndpointMessage(new ServiceEndpoint(serviceName, serviceUri)));
                MulticastClient.Send(message.ToJson());
            }
        }
    }
}