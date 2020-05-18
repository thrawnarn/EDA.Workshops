using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RPS.Tests
{
    public class HighScoreViewTestsIntegration
    {
        [Fact]
        public void IntegrationEvent()
        {
            var gamePlayedEvent = GameEvents(Guid.NewGuid(), "test", "alex@rpsgame.com", "lisa@rpsgame.com")
                .Rehydrate<GamePlayed>();

            Assert.Equal("lisa@rpsgame.com", gamePlayedEvent.Winner);
        }

        [Fact]
        public void HighScoreWithIntegrationEvent()
        {
            var playerOne = "alex@rpsgame.com";
            var playerTwo = "lisa@rpsgame.com";
            var playerThree = "julie@rpsgame.com";
            var state = Enumerable
                .Range(0, 20)
                .Select((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i}", playerOne, playerTwo).Rehydrate<GamePlayed>())
                .Concat(Enumerable.Range(0, 10)
                .Select((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i + 20}", playerTwo, playerOne).Rehydrate<GamePlayed>()))
                .Concat(Enumerable.Range(0, 15)
                .Select((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i + 30}", playerThree, playerOne).Rehydrate<GamePlayed>()))
                .Concat(Enumerable.Range(0, 5)
                .Select((x, i) => GameEvents(Guid.NewGuid(), $"Game_{i + 45}", playerTwo, playerThree).Rehydrate<GamePlayed>()))
                .Rehydrate<HighScoreView>();

            Assert.Equal(25, state.Rows.OrderBy(r => r.Rank).First().GamesWon);
        }

        public static IEvent[] GameEvents(Guid gameId, string title, string loosingPlayer, string winningPlayer)
            => new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = loosingPlayer, Rounds = 1, Title = title },
                new GameStarted { GameId = gameId, PlayerId = winningPlayer },
                new RoundStarted { GameId = gameId, Round = 1 },
                new HandShown { GameId = gameId, Hand = Hand.Paper, PlayerId = loosingPlayer },
                new HandShown { GameId = gameId, Hand = Hand.Rock, PlayerId = winningPlayer },
                new RoundEnded { GameId = gameId, Round = 1, Looser = loosingPlayer, Winner = winningPlayer },
                new GameEnded { GameId = gameId }
            };
    }
}
