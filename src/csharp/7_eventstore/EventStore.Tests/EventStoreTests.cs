using System;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace Subscription.Tests
{
    public class EventStoreTests
    {
        [Fact]
        public async Task DuplicateEventThrowsAsync()
        {
            var store = new InMemoryEventStore();
            var @event = new TestEvent(Guid.NewGuid());

            _ = await store.AppendToStreamAsync("test", new[] { @event });

            await Assert.ThrowsAsync<Exception>(() =>
                store.AppendToStreamAsync("test", new[] { @event })
            );
        }

        [Fact]
        public async Task WrongVersionThrowsAsync()
        {
            var store = new InMemoryEventStore();

            _ = await store.AppendToStreamAsync("test", 0, new[] { new TestEvent(Guid.NewGuid()) });

            await Assert.ThrowsAsync<DBConcurrencyException>(() =>
                store.AppendToStreamAsync("test", 0, new[] { new TestEvent(Guid.NewGuid()) })
            );
        }

        public class TestEvent : IEvent
        {
            public TestEvent(Guid eventId)
            {
                EventId = eventId;
            }

            public Guid EventId { get; }
        }
    }
}
