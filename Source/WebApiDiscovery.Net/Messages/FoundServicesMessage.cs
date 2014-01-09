using Krowiorsch.Model;

namespace Krowiorsch.Messages
{
    public class FoundServicesMessage
    {
        public FoundServicesMessage(ServiceEndpoint[] endpoints)
        {
            Endpoints = endpoints;
        }

        public ServiceEndpoint[] Endpoints { get; set; }
    }
}