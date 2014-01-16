using System;
using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Messages;

using NLog;

namespace Krowiorsch.Model
{
    public class ServiceCatalog
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly List<ServiceEndpointWithState> _knownServices = new List<ServiceEndpointWithState>();

        static readonly object SyncLock = new object();

        public ServiceCatalog()
            : this(TimeSpan.FromSeconds(10))
        {
        }

        public ServiceCatalog(TimeSpan serviceTimeout)
        {
            ServiceTimeout = serviceTimeout;
        }

        public ServiceEndpoint[] GetByName(string serviceName)
        {
            lock(SyncLock)
            {
                return _knownServices.Where(t => t.ServiceIdentifier.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Cast<ServiceEndpoint>().ToArray();
            }
        }

        public string[] GetKnownServices()
        {
            lock(SyncLock)
            {
                return _knownServices.Select(t => t.ServiceIdentifier).Distinct().ToArray();
            }
        }

        internal void Handle(RegisterEndpointMessage message)
        {
            lock(SyncLock)
            {
                var service = _knownServices.FirstOrDefault(t => t.Equals(t, message.Endpoint));

                if(service != null)
                {
                    service.DetectBeat();
                    return;
                }

                Logger.Debug("New Service Detected: {0} on {1}", message.Endpoint.ServiceIdentifier, message.Endpoint.Endpoint.ToString());

                _knownServices.Add(new ServiceEndpointWithState(message.Endpoint));
            }
        }

        internal void Handle(UnregisterEndpointMessage message)
        {
            lock(SyncLock)
            {
                _knownServices.RemoveAll(t => t.Equals(t, message.Endpoint));
            }
        }

        internal void CheckHeartbeat()
        {
            lock(SyncLock)
            {
                var deleted = _knownServices.RemoveAll(t => t.LastHeartbeat.HasValue && ( t.LastHeartbeat.Value - DateTime.Now ).Duration() > ServiceTimeout.Duration());
                if(deleted > 0)
                    Logger.Warn("Remove {0} services from Catalog", deleted);
            }
        }

        public TimeSpan ServiceTimeout { get; private set; }
    }
}