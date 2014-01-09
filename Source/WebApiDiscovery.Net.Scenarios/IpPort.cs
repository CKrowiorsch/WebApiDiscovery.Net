using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace Krowiorsch
{
    public static class IpPort
    {
        public static int GetNextPortAvailable(int basePort)
        {
            var globalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var objEndPoints = globalProperties.GetActiveTcpListeners();

            var bindedPorts = objEndPoints.Select(t => t.Port).Distinct().OrderBy(t => t);

            for (int i = basePort; i <= 65535; i++)
            {
                if (!bindedPorts.Contains(i))
                    return i;
            }

            throw new InvalidOperationException("No Port Available");
        }
    }
}