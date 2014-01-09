using System;
using System.Linq;

using Krowiorsch.Model;

namespace Krowiorsch.Selectors
{
    public class RandomSelector : ICanSelectServices
    {
        static readonly Random Rnd = new Random(Environment.TickCount);

        public WebApiServiceState Select(WebApiServiceState[] availableServices)
        {
            if (!availableServices.Any())
                return null;

            var count = availableServices.Count();

            return availableServices[Rnd.Next(0, count)];
        }
    }
}