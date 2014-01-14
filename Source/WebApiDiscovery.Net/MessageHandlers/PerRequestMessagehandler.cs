using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using NLog;

namespace Krowiorsch.MessageHandlers
{
    public class PerRequestMessagehandler : DelegatingHandler
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        readonly IDiscoveryClient _discoveryClient = new DiscoveryClient();
        readonly Uri _serviceUri;

        public PerRequestMessagehandler(Uri serviceUri, IDiscoveryClient client)
        {
            _serviceUri = serviceUri;
            _discoveryClient = client;

            InnerHandler = new HttpClientHandler();
        }

        public PerRequestMessagehandler(Uri serviceUri)
            :this(serviceUri, new DiscoveryClient())
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var serverUri = _discoveryClient.Discover(_serviceUri);
            request.RequestUri = new Uri(serverUri, request.RequestUri.PathAndQuery);
            return base.SendAsync(request, cancellationToken);
        }
    }
}