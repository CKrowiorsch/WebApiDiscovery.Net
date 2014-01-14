using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Krowiorsch.Impl;
using Krowiorsch.Messages;
using Krowiorsch.Model;
using Krowiorsch.Selectors;

using NLog;

namespace Krowiorsch
{
    public class DiscoveryClient : IDisposable, IDiscoveryClient
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static readonly ServiceCatalog Catalog = new ServiceCatalog();

        static Task _discoveryTask;
        static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        readonly ICanSelectServices _serviceSelector;

        static readonly MulticastClient _multicastClient = MulticastClientFactory.CreateForServiceDiscovery();

        static DiscoveryClient()
        {
        }

        public static void Initialize()
        {
            _discoveryTask = WatchAsync().ContinueWith(t => Logger.Warn("Discovery Task wurde beendet"));
        }

        public DiscoveryClient()
            : this(ServiceSelectors.RoundRobin)
        {

        }

        public DiscoveryClient(ICanSelectServices selector)
        {
            _serviceSelector = selector;
        }

        static Task WatchAsync()
        {
            Task.Factory.StartNew(() =>
            {
                while(!_cancellationTokenSource.IsCancellationRequested)
                {
                    Catalog.CheckHeartbeat();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }, _cancellationTokenSource.Token);
            
            return _multicastClient.ReceiveAsync(s =>
            {
                var message = Message.FromJson(s);
                var content = message.GetBodyMessage();

                if (content is RegisterEndpointMessage)
                    Catalog.Handle((RegisterEndpointMessage)content);

                if (content is UnregisterEndpointMessage)
                    Catalog.Handle((UnregisterEndpointMessage)content);
            });
        }


        public Uri Discover(Uri discoveryIdentifier)
        {
            if (discoveryIdentifier.Scheme.Equals("discover"))
                return DiscoverByServiceIdentifier(discoveryIdentifier.PathAndQuery);

            return discoveryIdentifier;
        }

        public Uri[] DiscoverAll(Uri discoveryIdentifier)
        {
            if (discoveryIdentifier.Scheme.Equals("discover"))
                return DiscoverAllByServiceIdentifier(discoveryIdentifier.PathAndQuery);

            return new[] { discoveryIdentifier };
        }

        public Uri DiscoverByServiceIdentifier(string serviceName)
        {
            var serviceEndpoints = Catalog.GetByName(serviceName);

            if (!serviceEndpoints.Any())
                return null;

            var selectedService = _serviceSelector.Select(serviceEndpoints.Cast<ServiceEndpointWithState>().ToArray());
            return selectedService == null ? null : selectedService.Endpoint;
        }

        public Uri[] DiscoverAllByServiceIdentifier(string serviceName)
        {
            var serviceEndpoints = Catalog.GetByName(serviceName);

            if (!serviceEndpoints.Any())
                return new Uri[0];

            return serviceEndpoints.Select(t => t.Endpoint).ToArray();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}