using Machine.Specifications;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace Krowiorsch.Model
{
    public class with_ServiceCatalog
    {
        Establish context = () =>
            _subject = new ServiceCatalog();

        protected static ServiceCatalog _subject;
    }
}