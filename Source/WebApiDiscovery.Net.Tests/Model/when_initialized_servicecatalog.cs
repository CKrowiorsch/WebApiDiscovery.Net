using System;

using Machine.Specifications;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Krowiorsch.Model
{
    public class when_initialized_servicecatalog
    {
        Because of = () => 
            _subject = new ServiceCatalog();

        It should_have_entries = () => 
            _subject.GetKnownServices().ShouldBeEmpty();

        It should_have_a_servicetimeout_of_10_s = () => 
            _subject.ServiceTimeout.ShouldEqual(TimeSpan.FromSeconds(10));

        protected static ServiceCatalog _subject;
    }
}