using System;

namespace Krowiorsch.Model
{
    public class ServiceEndpoint
    {
        public ServiceEndpoint(string serviceName, Uri endpoint)
        {
            ServiceName = serviceName;
            Endpoint = endpoint;
        }

        public string ServiceName { get; internal set; }

        public Uri Endpoint { get; internal set; }
    }
}