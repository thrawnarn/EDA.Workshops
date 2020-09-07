using System;

namespace Invariants.Tests
{
    public class GameState
    {
        public Guid Id { get; set; }
        public (Player PlayerOne, Player PlayerTwo) Players { get; set; }
        public int Round { get; set; }
        public int Rounds { get; set; }

        public GameState When(IEvent @event) => this;

        public GameState When(GameCreated @event)
        {
            Id = @event.GameId;
            Players = (new Player { Id = @event.PlayerId }, default);
            Rounds = @event.Rounds;
            return this;
        }
        public class Player
        {
            public string Id { get; set; }
            public Hand Hand { get; set; }
        }

    }

}
