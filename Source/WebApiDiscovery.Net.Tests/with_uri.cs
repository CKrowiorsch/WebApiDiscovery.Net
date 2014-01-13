using System;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Krowiorsch
{
    public class when_use_discovery_uri
    {
        Because of = () => 
            _result = new Uri("discover:servicename.withdot");

        It should_have_scheme_discover = () => 
            _result.Scheme.ShouldBeEqualIgnoringCase("discover");

        It should_have_servicename = () =>
            _result.PathAndQuery.ShouldBeEqualIgnoringCase("servicename.withdot");

        static Uri _result;
    }
}