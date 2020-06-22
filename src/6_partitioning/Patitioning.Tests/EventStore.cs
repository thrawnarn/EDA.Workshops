using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Patitioning.Tests
{
    public class EventStore
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<Event> database = new List<Event>();

        public IEnumerable<Event> Read(
          long firstEventSequenceNumber,
          long lastEventSequenceNumber)
          => database
            .Where(e =>
              e.SequenceNumber >= firstEventSequenceNumber &&
              e.SequenceNumber <= lastEventSequenceNumber)
            .OrderBy(e => e.SequenceNumber);

        public void Append<T>(string eventName, T @event)
        {
            var seqNumber = Interlocked.Increment(ref currentSequenceNumber);
            database.Add(
              new Event(
                seqNumber,
                DateTimeOffset.UtcNow,
                eventName,
                @event));
        }

        public struct Event
        {
            public long SequenceNumber { get; }
            public DateTimeOffset OccuredAt { get; }
            public string Name { get; }
            public object Content { get; }

            public Event(
              long sequenceNumber,
              DateTimeOffset occuredAt,
              string name,
              object content)
            {
                this.SequenceNumber = sequenceNumber;
                this.OccuredAt = occuredAt;
                this.Name = name;
                this.Content = content;
            }
        }
    }

    public static class EventStoreExtensions
    {
        public static void Append(this EventStore store, params IEvent[] events)
            => events.ToList().ForEach(e => store.Append(e.GetType().Name, e));
    }


    public static class EventStoreRoomExtensions
    { 
            public static string[] GetCheckedInRoomIds(this EventStore store)
            => store.Read(0, int.MaxValue)
                .Select(x => x.Content)
				.OfType<IRoomEvent>()
				.GroupBy(o => o.RoomId)
				.Where(o => o.Last() is RoomCheckedIn)
				.Select(o => o.Key)
				.ToArray();

        public static string[] GetRoomsToClean(this EventStore store)
            => Array.Empty<string>();

    }
}
