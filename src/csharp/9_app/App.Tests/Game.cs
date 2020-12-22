using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(CreateGame command, params IEvent[] events)
        => new[] {
                    new GameCreated
                    {
                               GameId = command.GameId,
                               PlayerId = command.PlayerId,
                               Title = command.Title,
                               Rounds = command.Rounds,
                               Created = DateTime.UtcNow,
                               Status = GameStatus.ReadyToStart
                    }
              };

        public static IEnumerable<IEvent> Handle(JoinGame command, GameState state)
        {
            if (state.Players.PlayerOne.Id == command.PlayerId)
                yield break;

            if (state.Players.PlayerTwo == default)
            {
                yield return new GameStarted { GameId = command.GameId, PlayerId = command.PlayerId };
                yield return new RoundStarted { GameId = command.GameId, Round = 1 };
            }
        }
    }

    public class CreateGame : ICommand
    {
        [Required]
        [ScaffoldColumn(false)]
        public Guid GameId { get; set; }

        [Required]
        public string PlayerId { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public int Rounds { get; set; }
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
        public Guid EventId { get; } = Guid.NewGuid();
    }

    public class RoundStarted : IEvent
    {
        public Guid GameId { get; set; }

        public int Round { get; set; }

        public string SourceId => GameId.ToString();
        public Guid EventId { get; } = Guid.NewGuid();

    }

    public class GameStarted : IEvent
    {
        public Guid GameId { get; set; }

        public string PlayerId { get; set; }

        public string SourceId => GameId.ToString();
        public Guid EventId { get; } = Guid.NewGuid();

    }

    public enum GameStatus
    {
        None = 0,
        ReadyToStart = 10,
        Started = 20,
        Ended = 50
    }

    public class EventData
    {
        public EventData(IEvent @event, long eventVersion, long eventPosition, Guid eventId, string eventName)
        {
            Event = @event;
            EventVersion = eventVersion;
            EventPosition = eventPosition;
            EventId = eventId;
            EventName = eventName;
        }

        public IEvent Event { get; }
        public long EventVersion { get; }
        public long EventPosition { get; }
        public Guid EventId { get; }
        public string EventName { get; }
    }

    public class JoinGame : ICommand
    {
        [ScaffoldColumn(false)]
        public Guid GameId { get; set; }

        public string PlayerId { get; set; }
    }

    public class GameState
    {
        public Guid Id { get; set; }
        public (Player PlayerOne, Player PlayerTwo) Players { get; set; }
        public int Round { get; set; }
        public int Rounds { get; set; }
        public GameStatus Status { get; set; }
        public GameState When(IEvent @event) => this;

        public GameState When(GameCreated @event)
        {
            Status = GameStatus.ReadyToStart;
            Id = @event.GameId;
            Players = (new Player { Id = @event.PlayerId }, default);
            Rounds = @event.Rounds;
            return this;
        }

        public GameState When(GameStarted @event)
        {
            Status = GameStatus.Started;
            Players = (Players.PlayerOne, new Player { Id = @event.PlayerId });
            return this;
        }

        public GameState When(RoundStarted @event)
        {
            Status = GameStatus.Started;
            Round = @event.Round;
            return this;
        }

        public class Player
        {
            public string Id { get; set; }
            public Hand Hand { get; set; }
        }

        public enum GameStatus
        {
            None = 0,
            ReadyToStart = 10,
            Started = 20,
            Ended = 50
        }
    }

    public enum Hand
    {
        None = 0,
        Rock = 10,
        Paper = 20,
        Scissors = 30
    }


    public interface IEvent
    {
        Guid EventId { get; }
    }

    public interface ICommand
    { }
}
