using System;
using System.Collections.Generic;

namespace RPS.Tests
{
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

        public GameState When(HandShown @event)
        {
            var p = @event.PlayerId switch
            {
                string id when id == Players.PlayerOne.Id => Players.PlayerOne,
                _ => Players.PlayerTwo
            };
            p.Hand = @event.Hand;

            return this;
        }

        public GameState When(GameEnded @event)
        {
            Status = GameStatus.Ended;
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
}
