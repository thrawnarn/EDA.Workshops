using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Query.Tests
{
    public class App
    {
        List<IEvent> history = new List<IEvent>();

        public void Given(params IEvent[] events) => history.AddRange(events);

        public Task<T> QueryAsync<T>(IQuery<T> q)
            where T : class
            => q switch
            {
                GameQuery query => Task.FromResult(history.Rehydrate<GamesView>().Games.Single(x => x.Key == query.GameId.ToString()).Value as T),
                _ => Task.FromResult(default(T))
            };
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
