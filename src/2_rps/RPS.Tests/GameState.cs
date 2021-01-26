using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPS.Tests
{
    public class GameState
    {
        public class Player
        {
            public string Id { get; set; }
            public Hand Hand { get; set; }
        }

        public List<Player> Players { get; set; } = new List<Player>();

        public GameStatus Status { get; set; }

        public int Rounds { get; set; }

        public Guid GameId { get; set; }

        public string Title { get; set; }
        public int Round { get; set; }


        public GameState When(GameStarted @event)
        {
            Players.Add(new Player()
            {
                Hand =   Hand.None,
                Id = @event.PlayerId
            });
            return this;
        }

        public GameState When(RoundStarted @event)
        {
            Round++;
            return this;
        }
        public GameState When(GameCreated @event)
        {
            Players.Add(new Player()
            {
                Hand = Hand.None,
                Id = @event.PlayerId
            });

            Title = @event.Title;
            GameId = @event.GameId;
            Rounds = @event.Rounds;
            Status = @event.Status;
            return this;
        }

        public GameState When(HandShown @event)
        {
            Players.SingleOrDefault(p => p.Id.Equals(@event.PlayerId)).Hand = @event.Hand;
            return this;
        }



        public GameState When(IEvent @event) => this;

        public enum GameStatus
        {
            None = 0,
            ReadyToStart = 10,
            Started = 20,
            Ended = 50
        }
    }

}
