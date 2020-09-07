using System;
using System.Linq;
using Xunit;

namespace Invariants.Tests
{
    public class GameTests
    {
        [Fact]
        public void CreatorCannotJoinGameWithState()
        {
            var gameId = Guid.NewGuid();

            //Given
            var state = new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" },
                }.Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new JoinGame { GameId = gameId, PlayerId = "test@tester.com" },
                state
                );

            //Then  
            Assert.False(events.Any());
        }

        [Fact]
        public void CreatorCannotJoinGameWithHistory()
        {
            var gameId = Guid.NewGuid();

            //Given
            var history = new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" },
                };

            //When
            var events = Game.Handle(
                new JoinGame { GameId = gameId, PlayerId = "test@tester.com" },
                history
                );

            //Then  
            Assert.False(events.Any());
        }

        [Fact]
        public void PlayerJoinsGameWithHistory()
        {
            var gameId = Guid.NewGuid();

            //Given
            var history = new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" },
                };

            //When
            var events = Game.Handle(
                new JoinGame { GameId = gameId, PlayerId = "fey@tester.com" },
                history
                );

            //Then  
            Assert.True(events.OfType<GameStarted>().Count() == 1);
            Assert.True(events.OfType<RoundStarted>().Count() == 1);
        }

        [Fact]
        public void PlayerJoinsGameWithState()
        {
            var gameId = Guid.NewGuid();

            //Given
            var state = new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" },
                }.Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new JoinGame { GameId = gameId, PlayerId = "fey@tester.com" },
                state
                );

            //Then  
            Assert.True(events.OfType<GameStarted>().Count() == 1);
            Assert.True(events.OfType<RoundStarted>().Count() == 1);
        }
    }
}
