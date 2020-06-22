using System;

namespace Subscription.Tests
{
    public interface IEvent
    {
        Guid EventId { get; }
    }

    public class GameStarted : IEvent
    {
        public Guid GameId { get; set; }

        public string PlayerId { get; set; }

        public string SourceId => GameId.ToString();
        public Guid EventId { get; set; } = Guid.NewGuid();
    }

    public class GameEnded : IEvent
    {
        public Guid GameId { get; set; }

        public string SourceId => GameId.ToString();

        public Guid EventId { get; set; } = Guid.NewGuid();
    }

    public class ResolvedEvent
    {
        public ResolvedEvent(string streamName, EventData @event)
        {
            StreamName = streamName;
            Event = @event;
        }
        public string StreamName { get; }
        public EventData Event { get; }
    }

    public class EventData
    {
        public EventData(IEvent @event, long eventVersion, long eventPosition, Guid eventId, string eventName)
        {
            Event = @event;
            EventVersion = eventVersion;
            EventPosition = eventPosition;
            EventId = eventId;
            EventName = eventName;
        }

        public IEvent Event { get; }
        public long EventVersion { get; }
        public long EventPosition { get; }
        public Guid EventId { get; }
        public string EventName { get; }
    }
}