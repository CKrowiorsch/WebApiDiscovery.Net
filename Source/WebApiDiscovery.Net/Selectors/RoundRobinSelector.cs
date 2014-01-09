using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Model;

namespace Krowiorsch.Selectors
{
    public class RoundRobinSelector : ICanSelectServices
    {
        readonly IDictionary<string, int> _lastPositions = new Dictionary<string, int>();

        public WebApiServiceState Select(WebApiServiceState[] availableServices)
        {
            var serviceIdentifier = availableServices.First().ServiceIdentifier;

            var lastPosition = _lastPositions.ContainsKey(serviceIdentifier) ? _lastPositions[serviceIdentifier] : 0;


            var newPosition = (lastPosition + 1) % (availableServices.Count());

            if(_lastPositions.ContainsKey(serviceIdentifier))
                _lastPositions[serviceIdentifier] = newPosition;
            else
                _lastPositions.Add(serviceIdentifier, newPosition);

            return availableServices[newPosition];
        }
    }
}