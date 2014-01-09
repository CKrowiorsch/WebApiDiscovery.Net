using Krowiorsch.Model;

namespace Krowiorsch.Messages
{
    public class UnregisterEndpointMessage
    {
        public UnregisterEndpointMessage(ServiceEndpoint endpoint)
        {
            Endpoint = endpoint;
        }

        public ServiceEndpoint Endpoint { get; set; }
    }
}