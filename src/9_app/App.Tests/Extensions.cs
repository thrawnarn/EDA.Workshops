using System.Collections.Generic;
using System.Linq;

namespace App.Tests
{
    public static class Extensions
    {
        public static TState Rehydrate<TState>(this IEnumerable<IEvent> events) where TState : new()
            => events.Aggregate(new TState(), (s, @event) => ((dynamic)s).When((dynamic)@event));
    }
}
