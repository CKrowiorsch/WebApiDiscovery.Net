namespace Krowiorsch.Selectors
{
    public static class ServiceSelectors
    {
         public static ICanSelectServices Random = new RandomSelector();
         public static ICanSelectServices RoundRobin = new RoundRobinSelector();
    }
}