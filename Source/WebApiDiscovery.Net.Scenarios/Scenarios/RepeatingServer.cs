using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Krowiorsch.Scenarios
{
    internal class RepeatingServer
    {
        public Task StartServer(TimeSpan interval, Action<long> action)
        {
            var disposable = Disposable.Empty;

            return Task.Factory.StartNew(() =>
            {
                disposable = Observable.Interval(interval).Subscribe(action);

                while(true)
                {
                    
                }
            });
        } 
    }
}