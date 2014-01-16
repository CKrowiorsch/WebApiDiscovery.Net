using System;

namespace Krowiorsch
{
    public interface IDiscoveryClient
    {
        Uri Discover(Uri discoveryIdentifier);

        Uri[] DiscoverAll(Uri discoveryIdentifier);

        string[] KnownServices();
    }
}