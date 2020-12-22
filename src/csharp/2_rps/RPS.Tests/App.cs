using System;
using System.Collections.Generic;
using static RPS.Tests.GameState;

namespace RPS.Tests
{

    public interface IEvent
    {
        string SourceId { get; }
        IDictionary<string, string> Meta { get; }
    }

    public class CreateGame
    {
        public Guid GameId { get; set; }
        public string PlayerId { get; set; }
        public string Title { get; set; }
        public int Rounds { get; set; }
    }

    public class JoinGame
    {
        public Guid GameId { get; set; }

        public string PlayerId { get; set; }
    }

    public class PlayGame
    {
        public Guid GameId { get; set; }
        public Hand Hand { get; set; }
        public string PlayerId { get; set; }
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
        public IDictionary<string, string> Meta { get; set; }
    }

    public class RoundStarted : IEvent
    {
        public Guid GameId { get; set; }

        public int Round { get; set; }

        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

    public class GameStarted : IEvent
    {
        public Guid GameId { get; set; }

        public string PlayerId { get; set; }

        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

    public class GameEnded : IEvent
    {
        public Guid GameId { get; set; }

        public string SourceId => GameId.ToString();

        public IDictionary<string, string> Meta { get; set; }
    }

    public class RoundTied : IEvent
    {
        public Guid GameId { get; set; }
        public int Round { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

    public class RoundEnded : IEvent
    {
        public Guid GameId { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public int Round { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

    public class HandShown : IEvent
    {
        public Guid GameId { get; set; }
        public string PlayerId { get; set; }
        public Hand Hand { get; set; }

        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

    public class GamePlayed : IEvent
    {
        public Guid GameId { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

    public enum Hand
    {
        None = 0,
        Rock = 10,
        Paper = 20,
        Scissors = 30
    }

    public enum RoundResult
    {
        Tied = 10,
        Won = 20,
        Lost = 30
    }

}