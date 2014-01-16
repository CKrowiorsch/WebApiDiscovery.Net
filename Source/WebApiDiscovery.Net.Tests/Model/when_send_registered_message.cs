using System;
using System.Linq;

using Krowiorsch.Messages;

using Machine.Specifications;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Krowiorsch.Model
{
    public class when_send_one_registered_message : with_ServiceCatalog
    {
        Because of = () => 
            _subject.Handle(new RegisterEndpointMessage(new ServiceEndpoint("test", new Uri("local:1"))));

        It should_know_the_service = () => 
            _subject.GetKnownServices().ShouldContainOnly("test");

        It should_have_one_endpoint_for_test = () => 
            _subject.GetByName("test").Select(t => t.Endpoint).ShouldContainOnly(new Uri("local:1"));
    }
}