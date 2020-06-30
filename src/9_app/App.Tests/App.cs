using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tests
{
    public class App
    {
        private readonly InMemoryEventStore store;
        private readonly Func<IEvent[], Task> pub;

        public App(InMemoryEventStore store, Func<IEvent[], Task> pub)
        {
            this.store = store;
            this.pub = pub;
        }

        public async Task Dispatch(ICommand command)
        {
            async Task subsciption(IEvent[] events) {
                _ = await store.AppendToStreamAsync("games", events);
                await pub(events);
            };

            Func<InMemoryEventStore, Task> commandHandler = command switch
            {
                CreateGame cmd => (store) => Execute(store, $"game-{cmd.GameId}", events => Game.Handle(cmd, events).ToArray(), subsciption),
                JoinGame cmd => (store) => Execute<GameState>(store, $"game-{cmd.GameId}", state => Game.Handle(cmd, state).ToArray(), subsciption),
                _ => (store) => throw new ArgumentException($"Could not dispatch {command.GetType()}")
            };

            await commandHandler(store);
        }

        private static Task Execute<TState>(
            InMemoryEventStore store,
            string streamName,
            Func<TState, IEvent[]> f,
            Func<IEvent[], Task> pub)
            where TState : class, new()
            => Execute(store, streamName, data => f(data.Select(e => e.Event).Rehydrate<TState>()), pub);

        private static Task Execute(
            InMemoryEventStore store,
            string streamName,
            Func<IEvent[], IEvent[]> f,
            Func<IEvent[], Task> pub)
            => Execute(store, streamName, data => f(data.Select(e => e.Event).ToArray()), pub);

        private static async Task Execute(
            InMemoryEventStore store,
            string streamName,
            Func<EventData[], IEvent[]> f,
            Func<IEvent[], Task> pub)
        {
            var r = await store.LoadEventStreamAsync(streamName, 0);
            var events = f(r.Events.ToArray());
            _ = await store.AppendToStreamAsync(streamName, r.Version, events);
            await pub(events);
        }
    }
}
