using System;
using System.Collections.Generic;

namespace RPS.Tests
{
    public class GamePlayed : IEvent
    {
        private string Player1;
        private string Player2;
        private int Player1Wins;
        private int Player2Wins;
        public int CurrentRound { get; set; }
        public GamePlayed When(GameCreated @event)
        {
            Player1 = @event.PlayerId;

            return this;
        }

        public GamePlayed When(GameStarted @event)
        {
            Player2 = @event.PlayerId;
            return this;
        }

        public GamePlayed When(RoundStarted @event)
        {
            CurrentRound = @event.Round;
            Rounds++;
            return this;
        }

        public GamePlayed When(HandShown @event)
        {
            return this;
        }

        public GamePlayed When(RoundEnded @event)
        {
            if (@event.Winner == Player1)
            {
                Player1Wins++;
            }
            else
            {
                Player2Wins++;
            }

            return this;
        }

        public GamePlayed When(GameEnded @event)
        {
            Winner = Player1Wins > Player2Wins ? Player1 : Player2;
            Looser = Player1Wins < Player2Wins ? Player1 : Player2;

            return this;
        }


        public GamePlayed When(IEvent @event) => this;
        public Guid GameId { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

}
