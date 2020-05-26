using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPS.Tests
{
    public class GameStateChangeTests
    {
        [Fact]
        public void CreateGame()
        {
            //Given
            var state = Array.Empty<IEvent>()
                .Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new CreateGame { Rounds = 3, GameId = Guid.NewGuid(), PlayerId = "tester", Title = "Test Game" },
                state);

            //Then  
            Assert.True(events.OfType<GameCreated>().Any());
        }

        [Fact]
        public void JoinGame()
        {
            var gameId = Guid.NewGuid();

            //Given
            var state = new[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" }
                }.Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new JoinGame { GameId = gameId, PlayerId = "test2@tester.com" },
                state);

            //Then  
            Assert.True(events.OfType<GameStarted>().Any());
            Assert.True(events.OfType<RoundStarted>().Any());
        }

        [Fact]
        public void JoinGameAsCreator()
        {
            var gameId = Guid.NewGuid();

            //Given
            var state = new[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" }
            }.Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new JoinGame { GameId = gameId, PlayerId = "test@tester.com" },
                state);

            //Then  
            Assert.False(events.Any());
        }
    }
}
