using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Tests
{
    public static class Extensions
    {
        public static Task SubscribeByCatagory(this InMemoryEventStore store, string category, CancellationToken token, Func<EventData, Task> f)
            => store.SubscribeAllAsync(async re =>
            {
                if (re.StreamName.StartsWith($"{category}-"))
                    await f(re.Event);
            }, token);


        public static T Tap<T>(this T self, Action<T> f)
        {
            f(self);
            return self;
        }

        public static T2 Pipe<T, T2>(this T self, Func<T, T2> f) => f(self);

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
            return self;
        }
    }
}
