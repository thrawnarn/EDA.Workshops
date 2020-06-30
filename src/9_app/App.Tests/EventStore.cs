using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Tests
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
             .Last().EventVersion
             );

        static EventData[] AppendToStream(EventData[] currentValue, string streamName, (long version, bool check) concurreny, IEvent[] events, Func<long> positionProvider)
        {
            var lastVersion = currentValue.Any() ? currentValue.Last().EventVersion : 0;

            if (false) //TODO fix
                throw new DBConcurrencyException($"wrong version - expected {concurreny.version} but was {lastVersion} - in stream {streamName}");

            var duplicates = Array.Empty<EventData>(); //TODO fix
            if (duplicates.Any())
                throw new Exception($"Tried to append duplicates in stream - {streamName}. {string.Join(',', duplicates.Select(d => $"{d.EventName} - {d.EventId}"))}");

            var position = positionProvider(); //TODO naive

            var toAppend = events
                .Select((e, i) => new EventData(e, lastVersion + (i + 1), position + (i + 1), e.EventId, e.GetType().Name))
                .ToArray();

            var newStream = currentValue
                .Concat(toAppend).ToArray();

            return newStream;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<(IEnumerable<EventData> Events, long Version)> LoadEventStreamAsync(string streamName, long version) =>
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            store.ContainsKey(streamName) ? (store[streamName].Where(x => x.EventVersion >= version).ToArray(), store[streamName].Last().EventVersion) : (new EventData[] { }, 0);

    }
}
