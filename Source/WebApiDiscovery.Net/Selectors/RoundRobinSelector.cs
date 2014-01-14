using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Model;

namespace Krowiorsch.Selectors
{
    public class RoundRobinSelector : ICanSelectServices
    {
        readonly IDictionary<string, int> _lastPositions = new Dictionary<string, int>();

        public ServiceEndpointWithState Select(ServiceEndpointWithState[] availableServicesEndpointWith)
        {
            var serviceIdentifier = availableServicesEndpointWith.First().ServiceIdentifier;

            var lastPosition = _lastPositions.ContainsKey(serviceIdentifier) ? _lastPositions[serviceIdentifier] : 0;


            var newPosition = (lastPosition + 1) % (availableServicesEndpointWith.Count());

            if(_lastPositions.ContainsKey(serviceIdentifier))
                _lastPositions[serviceIdentifier] = newPosition;
            else
                _lastPositions.Add(serviceIdentifier, newPosition);

            return availableServicesEndpointWith[newPosition];
        }
    }
}