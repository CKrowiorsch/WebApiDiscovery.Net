using System;
using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Model;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Krowiorsch.Selectors
{
    public class with_roundRobinSelector
    {
        Establish context = () =>
         _subject = new RoundRobinSelector();

        protected static RoundRobinSelector _subject;
    }

    public class when_select_with_one_entry : with_roundRobinSelector
    {
        Establish context = () =>
        {
            _endpointWithState = new[] { new ServiceEndpointWithState("test", new Uri("local:1")), new ServiceEndpointWithState("test", new Uri("local:2")) };
        };

        Because of = () => 
            _result = Enumerable.Range(1, 5).Select(i => _subject.Select(_endpointWithState)).ToList();

        It should_have_5_results = () => 
            _result.Count().ShouldEqual(5);

        It should_have_local_1_on_odd_places = () => 
            _result.Where((s, i) => i % 2 == 1).Any(t => t.Endpoint.Equals(new Uri("local:1"))).ShouldBeTrue();

        It should_have_local_2_on_even_places = () =>
            _result.Where((s, i) => i % 2 == 0).Any(t => t.Endpoint.Equals(new Uri("local:2"))).ShouldBeTrue();

        static ServiceEndpointWithState[] _endpointWithState;

        static IList<ServiceEndpointWithState> _result;
    }
}