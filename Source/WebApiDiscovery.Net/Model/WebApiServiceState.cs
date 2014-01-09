using System;

namespace Krowiorsch.Model
{
    public class WebApiServiceState
    {
        public WebApiServiceState(string serviceIdentifier, Uri serviceUri)
        {
            ServiceIdentifier = serviceIdentifier;
            ServiceUri = serviceUri;
        }

        public string ServiceIdentifier { get; set; }

        public Uri ServiceUri { get; set; }
    }
}