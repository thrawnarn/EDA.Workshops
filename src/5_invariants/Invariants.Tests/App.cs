using System;
using System.Collections.Generic;

namespace Invariants.Tests
{

    public interface IEvent
    {
        string SourceId { get; }
        IDictionary<string, string> Meta { get; }
    }

    public class JoinGame
    {
        public Guid GameId { get; set; }

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

    public enum Hand
    {
        None = 0,
        Rock = 10,
        Paper = 20,
        Scissors = 30
    }

    public enum GameStatus
    {
        None = 0,
        ReadyToStart = 10,
        Started = 20,
        Ended = 50
    }

}
