using Krowiorsch.Model;

namespace Krowiorsch.Messages
{
    public class RegisterEndpointMessage
    {
        public RegisterEndpointMessage(ServiceEndpoint endpoint)
        {
            Endpoint = endpoint;
        }

        public ServiceEndpoint Endpoint { get; set; }
    }
}