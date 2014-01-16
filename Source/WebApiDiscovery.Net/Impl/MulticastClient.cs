using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krowiorsch.Impl
{
    public class MulticastClient
    {
        public IPAddress MulticastAdress { get; internal set; }

        public int Port { get; internal set; }

        public MulticastClient(int port, IPAddress multicastAdress)
        {
            Port = port;
            MulticastAdress = multicastAdress;
        }

        public void Send(string data)
        {
            SendAsync(data).Wait();
        }

        public Task SendAsync(string data)
        {
            return Task.Factory.StartNew(() =>
            {
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach(var ipAddress in localIPs.Where(a => a.AddressFamily == AddressFamily.InterNetwork))
                {
                    var mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    mcastSocket.Bind(new IPEndPoint(ipAddress, 0));
                    var mcastOption = new MulticastOption(MulticastAdress);
                    mcastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, mcastOption);
                    var endPoint = new IPEndPoint(MulticastAdress, Port);
                    mcastSocket.SendTo(Encoding.ASCII.GetBytes(data), endPoint);
                }
            });
        }

        public Task ReceiveAsync(Action<string> onData)
        {
            var cancellationToken = new CancellationToken();

            return Task.Factory.StartNew(() =>
            {
                var multicastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                EndPoint localEndpoint = new IPEndPoint(IPAddress.Any, Port);

                multicastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                multicastSocket.ExclusiveAddressUse = false;
                multicastSocket.Bind(localEndpoint);

                var multicastOption = new MulticastOption(MulticastAdress);

                multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

                var bytes = new byte[1024];
                var remoteEndpoint = (EndPoint)new IPEndPoint(IPAddress.Any, 0);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        int length = multicastSocket.ReceiveFrom(bytes, ref remoteEndpoint);
                        onData(Encoding.ASCII.GetString(bytes, 0, length));
                    }

                    multicastSocket.Close();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }, cancellationToken);
        }
    }
}