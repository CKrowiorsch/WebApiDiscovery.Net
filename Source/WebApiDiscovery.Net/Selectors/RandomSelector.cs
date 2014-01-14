using System;
using System.Linq;

using Krowiorsch.Model;

namespace Krowiorsch.Selectors
{
    public class RandomSelector : ICanSelectServices
    {
        static readonly Random Rnd = new Random(Environment.TickCount);

        public ServiceEndpointWithState Select(ServiceEndpointWithState[] availableServicesEndpointWith)
        {
            if (!availableServicesEndpointWith.Any())
                return null;

            var count = availableServicesEndpointWith.Count();

            return availableServicesEndpointWith[Rnd.Next(0, count)];
        }
    }
}