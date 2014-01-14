using System;
using System.Collections.Generic;

namespace Krowiorsch.Model
{
    public class ServiceEndpoint : IEqualityComparer<ServiceEndpoint>
    {
        public ServiceEndpoint(string serviceIdentifier, Uri endpoint)
        {
            ServiceIdentifier = serviceIdentifier;
            Endpoint = endpoint;
        }

        public string ServiceIdentifier { get; internal set; }

        public Uri Endpoint { get; internal set; }

        public bool Equals(ServiceEndpoint x, ServiceEndpoint y)
        {
            if (x == null || y == null)
                return false;

            return x.ServiceIdentifier.Equals(y.ServiceIdentifier, StringComparison.OrdinalIgnoreCase) && x.Endpoint.Equals(y.Endpoint);
        }

        public int GetHashCode(ServiceEndpoint obj)
        {
            return obj.Endpoint.GetHashCode() + obj.ServiceIdentifier.GetHashCode();
        }
    }
}