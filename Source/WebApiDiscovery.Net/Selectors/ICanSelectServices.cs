using Krowiorsch.Model;

namespace Krowiorsch.Selectors
{
    public interface ICanSelectServices
    {
        WebApiServiceState Select(WebApiServiceState[] availableServices);
    }
}