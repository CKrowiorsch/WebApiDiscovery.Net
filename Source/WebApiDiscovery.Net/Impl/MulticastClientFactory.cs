using System.Net;

namespace Krowiorsch.Impl
{
    public static class MulticastClientFactory
    {
        public static MulticastClient CreateForServiceDiscovery()
        {
            return new MulticastClient(13371, IPAddress.Parse("224.168.100.5"));
        }
    }
}