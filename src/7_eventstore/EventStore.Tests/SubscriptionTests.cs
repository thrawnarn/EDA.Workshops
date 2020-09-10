using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Subscription.Tests
{
    public class SubscriptionTests
    {
        [Trait("Category", "Integration")]
        [Fact]
        public async Task SubscribeByCatagoryAsync()
        {
            var store = new InMemoryEventStore();
            await store.SubscribeByCatagory("game", CancellationToken.None, ed => store.AppendToStreamAsync("projection-ongoing-games", new[] { ed.Event }));

            var e1 = new GameStarted { GameId = Guid.NewGuid(), PlayerId = "lisa@rps.com" };
            await store.AppendToStreamAsync($"game-{e1.GameId}", new[] { e1 });
            var e2 = new GameStarted { GameId = Guid.NewGuid(), PlayerId = "rob@rps.com" };
            await store.AppendToStreamAsync($"game-{e2.GameId}", new[] { e2 });

            await Task.Delay(600);

            var r = await store.LoadEventStreamAsync("projection-ongoing-games", 0);

            Assert.Equal(2, r.Events.Count());
        }
    }
}
