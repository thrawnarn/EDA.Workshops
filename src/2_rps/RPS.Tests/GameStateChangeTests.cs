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

        [Fact]
        public void Handshown()
        {
            var gameId = Guid.NewGuid();

            //Given
            var state = new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" },
                new GameStarted { GameId = gameId, PlayerId = "foo@tester.com" },
                new RoundStarted { GameId = gameId, Round = 1 }
                }.Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new PlayGame { GameId = gameId, PlayerId = "foo@tester.com", Hand = Hand.Paper },
                state
                );

            //Then  
            Assert.True(events.OfType<HandShown>().Any());
        }

        [Fact]
        public void GameEnd()
        {
            var gameId = Guid.NewGuid();

            //Given
            var state = new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "test@tester.com", Rounds = 1, Title = "test game" },
                new GameStarted { GameId = gameId, PlayerId = "foo@tester.com" },
                new RoundStarted { GameId = gameId, Round = 1 },
                new HandShown { GameId = gameId, PlayerId = "test@tester.com", Hand = Hand.Paper }
            }.Rehydrate<GameState>();

            //When
            var events = Game.Handle(
                new PlayGame { GameId = gameId, PlayerId = "foo@tester.com", Hand = Hand.Rock },
                state
                );

            //Then  
            Assert.True(events.OfType<RoundEnded>().All(x => x.Winner == "test@tester.com"), "Wrong winner");
            Assert.True(events.OfType<GameEnded>().Any(), "no game end");
        }
    }
}
