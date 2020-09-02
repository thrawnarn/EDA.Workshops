using System;
using System.Collections.Generic;
using System.Linq;

namespace RPS.Tests
{
    public class GamePlayed : IEvent
    {
        Dictionary<string, int> g = new Dictionary<string, int>();
        public GamePlayed When(IEvent @event) => this;
        public GamePlayed When(GameCreated @event)
        {
            GameId = @event.GameId;
            Rounds = @event.Rounds;
            g.Add(@event.PlayerId, 0);
            return this;
        }

        public GamePlayed When(GameStarted @event)
        {
            g.Add(@event.PlayerId, 0);
            return this;
        }

        public GamePlayed When(RoundEnded @event)
        {
            g[@event.Winner]++;

            if (@event.Round == Rounds)
            {
                Winner = g
                 .OrderByDescending(x => x.Value)
                 .First()
                 .Key;
                Looser = g
                    .First(x => x.Key != Winner)
                    .Key;
            }
            
            return this;
        }

        public Guid GameId { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

}
