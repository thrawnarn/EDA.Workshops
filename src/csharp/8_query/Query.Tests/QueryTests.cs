using System;
using System.Threading.Tasks;
using Xunit;

namespace Query.Tests
{
    public class QueryTests
    {
        [Fact]
        public async Task GameViewById()
        {
            var app = new App();
            var gameId = Guid.NewGuid();
            var gameId2 = Guid.NewGuid();

            //GIVEN
            app.Given(new IEvent[] {
                new GameCreated { GameId = gameId, PlayerId = "alex@rps.com", Rounds = 1, Title = "Game #1" },
                new GameStarted { GameId = gameId, PlayerId = "sue@rps.com" },
                new GameEnded { GameId = gameId },
                new GameCreated { GameId = gameId2, PlayerId = "sue@rps.com", Rounds = 1, Title = "Game #2" },
                new GameStarted { GameId = gameId2, PlayerId = "joe@rps.com" },
                new GameEnded { GameId = gameId2 }
            });

            //WHEN
            var gameView = await app.QueryAsync(new GameQuery { GameId = gameId2 });

            //THEN
            Assert.Equal(GameStatus.Ended.ToString(), gameView.Status);
        }
    }
}
