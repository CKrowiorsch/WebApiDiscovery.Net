using System;
using System.Collections.Concurrent;
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
    public class DiscoveryClient : IDisposable
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static readonly ConcurrentDictionary<string, WebApiServiceState[]> _services
            = new ConcurrentDictionary<string, WebApiServiceState[]>();

        Task _discoveryTask;
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        readonly ICanSelectServices _serviceSelector;

        readonly MulticastClient _multicastClient = MulticastClientFactory.CreateForServiceDiscovery();

        public DiscoveryClient()
            : this(ServiceSelectors.RoundRobin)
        {

        }

        public DiscoveryClient(ICanSelectServices selector)
        {
            _serviceSelector = selector;
            _discoveryTask = WatchAsync().ContinueWith(t => Logger.Warn("Discovery Task wurde beendet"));
        }

        Task WatchAsync()
        {
            return _multicastClient.ReceiveAsync(s =>
            {
                var message = Message.FromJson(s);

                var content = message.GetBodyMessage();

                if (content is RegisterEndpointMessage)
                {
                    var registerMessage = (content as RegisterEndpointMessage);

                    var state = new WebApiServiceState(registerMessage.Endpoint.ServiceName, registerMessage.Endpoint.Endpoint);

                    _services.AddOrUpdate(registerMessage.Endpoint.ServiceName,
                        new[] { state },
                        (key, existingValue) =>
                        {
                            if (existingValue.Any(t => t.ServiceUri.Equals(state.ServiceUri)))
                                return existingValue;

                            Logger.Info("Register new service ... name:{0} uri:{1}", state.ServiceIdentifier, state.ServiceUri);

                            return existingValue.Union(new[] { state }).ToArray();
                        });
                }

                if (content is UnregisterEndpointMessage)
                {
                    var unRegisterMessage = (content as UnregisterEndpointMessage);
                    _services[unRegisterMessage.Endpoint.ServiceName] = _services[unRegisterMessage.Endpoint.ServiceName].Where(t => !t.ServiceUri.Equals(unRegisterMessage.Endpoint.Endpoint)).ToArray();
                }

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
            if (!_services.ContainsKey(serviceName))
                return null;

            var availableServices = _services[serviceName].ToArray();

            if (!availableServices.Any())
                return null;

            var selectedService = _serviceSelector.Select(availableServices);
            return selectedService == null ? null : selectedService.ServiceUri;
        }

        public Uri[] DiscoverAllByServiceIdentifier(string serviceName)
        {
            if (!_services.ContainsKey(serviceName))
                return null;

            return _services[serviceName].Select(t => t.ServiceUri).ToArray();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}