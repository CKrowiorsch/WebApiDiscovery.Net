namespace Krowiorsch.Messages
{
    public class ResolveServiceMessage
    {
        public ResolveServiceMessage(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; set; }
    }
}