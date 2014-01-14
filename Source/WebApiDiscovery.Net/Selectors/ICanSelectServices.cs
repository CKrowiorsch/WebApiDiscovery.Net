using Krowiorsch.Model;

namespace Krowiorsch.Selectors
{
    public interface ICanSelectServices
    {
        ServiceEndpointWithState Select(ServiceEndpointWithState[] availableServicesEndpointWith);
    }
}