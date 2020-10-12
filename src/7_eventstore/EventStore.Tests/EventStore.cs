using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscription.Tests
{
    public class InMemoryEventStore
    {
        readonly ConcurrentDictionary<string, EventData[]> innerStore = new ConcurrentDictionary<string, EventData[]>();
        IDictionary<string, EventData[]> store => innerStore;

        public Task<long> AppendToStreamAsync(string streamName, IEvent[] events)
            => AppendToStreamAsync(streamName, (default, false), events);

        public Task<long> AppendToStreamAsync(string streamName, long version, IEvent[] events)
         => AppendToStreamAsync(streamName, (version, true), events);

        public Task<long> AppendToStreamAsync(string streamName, (long version, bool check) concurreny, IEvent[] events)
         => Task.FromResult(innerStore.AddOrUpdate(
                    streamName,
                    key => AppendToStream(Array.Empty<EventData>(), key, concurreny, events, () => store.Values.Count()),
                    (key, value) => AppendToStream(value, key, concurreny, events, () => store.Values.Count()))
             .LastOrDefault().Pipe(x => x == null ? concurreny.version : x.EventVersion)
             );

        static EventData[] AppendToStream(EventData[] currentValue, string streamName, (long version, bool check) concurreny, IEvent[] events, Func<long> positionProvider)
        {
            var lastVersion = currentValue.Any() ? currentValue.Last().EventVersion : 0;

            if (concurreny.check && lastVersion != concurreny.version)
                throw new DBConcurrencyException($"wrong version - expected {concurreny.version} but was {lastVersion} - in stream {streamName}");

            var duplicates = currentValue.Where(x => events.Any(e => e.EventId == x.EventId));
            if (duplicates.Any())
                throw new Exception($"Tried to append duplicates in stream - {streamName}. {string.Join(',', duplicates.Select(d => $"{d.EventName} - {d.EventId}"))}");

            var position = positionProvider();

            var toAppend = events
                .Select((e, i) => new EventData(e, lastVersion + (i + 1), position + (i + 1), e.EventId, e.GetType().Name))
                .ToArray();

            var newStream = currentValue
                .Concat(toAppend).ToArray();

            l.ForEach(x => toAppend.ForEach(ed => x.Item2.Enqueue(new ResolvedEvent(streamName, ed))));

            return newStream;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<(IEnumerable<EventData> Events, long Version)> LoadEventStreamAsync(string streamName, long version) =>
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            store.ContainsKey(streamName) ? (store[streamName].Where(x => x.EventVersion >= version).ToArray(), store[streamName].Last().EventVersion) : (new EventData[] { }, 0);

        private static List<(Func<ResolvedEvent, Task>, Queue<ResolvedEvent>)> l = new List<(Func<ResolvedEvent, Task>, Queue<ResolvedEvent>)>();

        public Task SubscribeAllAsync(Func<ResolvedEvent, Task> f, CancellationToken ct, long position = 0, int pollInterval = 500)
        {
            var q = new Queue<ResolvedEvent>(innerStore
                .SelectMany(x => x.Value.Select(v => new { Event = v, Position = v.EventPosition, StreamName = x.Key })
                .OrderBy(x => x.Position)
                .Skip((int)position)
                .Select(x => new ResolvedEvent(x.StreamName, x.Event))));

            l.Add((f, q));

            return Task.Factory.StartNew(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    while (q.Any() && !ct.IsCancellationRequested)
                    {
                        var e = q.Peek();
                        await f(e);
                        q.Dequeue();
                    }
                    await Task.Delay(pollInterval, ct);
                }
            }, ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        }
    }
}
