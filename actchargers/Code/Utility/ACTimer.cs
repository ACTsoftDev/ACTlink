using System;
using System.Threading;
using System.Threading.Tasks;

namespace actchargers
{
    /// <summary>
    /// Timer callback.
    /// </summary>
    public delegate void TimerCallback(object state);


    /// <summary>
    /// Act Charger timer.
    /// </summary>
    public class ACTimer : CancellationTokenSource
    {
        public ACTimer(TimerCallback callback, object state, int dueTime, int period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
            {
                var tuple = (Tuple<TimerCallback, object>)s;

                while (true)
                {
                    if (IsCancellationRequested)
                        break;
                    await Task.Run(() => tuple.Item1(tuple.Item2));
                    await Task.Delay(period);
                }

            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public new void Dispose()
        {
            Cancel();
        }
    }
}
