using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Tests
{
    public class App
    {
        List<IEvent> history = new List<IEvent>();

        public void Given(params IEvent[] events) => history.AddRange(events);

        public Task<GameView> QueryAsync(GameQuery q)
        {
            return Task.FromResult(history.Rehydrate<GamesView>().Games[q.GameId.ToString()]);
        }

        public Task<T> QueryAsync<T>(IQuery<T> q)
            => Task.FromResult<T>(default);
    }

    public interface IEvent
    {
        Guid EventId { get; }
    }

    public class GameCreated : IEvent
    {
        public Guid GameId { get; set; }
        public string PlayerId { get; set; }
        public string Title { get; set; }
        public int Rounds { get; set; }
        public DateTime Created { get; set; }
        public GameStatus Status { get; set; } = GameStatus.Started;
        public string SourceId => GameId.ToString();
        public Guid EventId { get; set; } = Guid.NewGuid();

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

    public enum GameStatus
    {
        None = 0,
        ReadyToStart = 10,
        Started = 20,
        Ended = 50
    }

    public interface IQuery<T>
    { }

    public class GameQuery : IQuery<GameView>
    {
        public Guid GameId { get; set; }
    }
}
