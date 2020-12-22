using System;
using System.Collections.Generic;

namespace RPS.Tests
{
    public class GamePlayed : IEvent
    {
        public GamePlayed When(IEvent @event) => this;
        public Guid GameId { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Looser { get; set; }
        public string SourceId => GameId.ToString();
        public IDictionary<string, string> Meta { get; set; }
    }

}
