using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task CreateGame()
        {
            var store = new InMemoryEventStore();
            var collected = new List<IEvent>();
            var app = new App(store, events => { 
                collected.AddRange(events); 
                return Task.CompletedTask; 
            });

            //When
            await app.Dispatch(
                new CreateGame { GameId = Guid.NewGuid(), PlayerId = "cassy@rps.com", Rounds = 1, Title = "test" });
            //Then
            Assert.True(collected.OfType<GameCreated>().Count() == 1);
        }

        [Fact]
        public async Task JoiningNonExistentGame()
        {
            var store = new InMemoryEventStore();
            var collected = new List<IEvent>();
            var app = new App(store, events => {
                collected.AddRange(events);
                return Task.CompletedTask;
            });

            //When
            await app.Dispatch(
                new JoinGame { GameId = Guid.NewGuid(), PlayerId = "cassy@rps.com" });
            //Then
            Assert.False(collected.Any());
        }
    }
}
