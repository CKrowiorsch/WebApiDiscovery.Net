using System;

namespace Krowiorsch.Model
{
    public class ServiceEndpointWithState : ServiceEndpoint
    {
        public ServiceEndpointWithState(ServiceEndpoint serviceEndpoint)
            : this(serviceEndpoint.ServiceIdentifier, serviceEndpoint.Endpoint, DateTime.Now)
        {
        }

        public ServiceEndpointWithState(string serviceIdentifier, Uri serviceUri)
            : this(serviceIdentifier, serviceUri, DateTime.Now)
        {
            
        }

        public ServiceEndpointWithState(string serviceIdentifier, Uri serviceUri, DateTime lastHeartbeat)
            : base(serviceIdentifier, serviceUri)
        {
            LastHeartbeat = lastHeartbeat;
        }

        public DateTime? LastHeartbeat { get; set; }

        public void DetectBeat()
        {
            LastHeartbeat = DateTime.Now;
        }
    }
}