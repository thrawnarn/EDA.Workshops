using System;
using System.Collections.Generic;
using System.Linq;

namespace RPS.Tests
{
    public class HighScoreViewHistory
    {
        List<GamePlayed> history = new List<GamePlayed>();

        public HighScoreViewHistory When(IEvent @event) => this;
        public HighScoreViewHistory When(GamePlayed @event)
        {
            history.Add(@event);
            return this;
        }

        public ScoreRow[] Rows =>
            history
                .GroupBy(x => x.Winner)
                .Concat(history.GroupBy(x => x.Looser))
                .GroupBy(x => x.Key)
                .Select((x, i) => new ScoreRow { PlayerId = x.Key, GamesWon = history.Count(h => h.Winner == x.Key) })
                .OrderByDescending(x => x.GamesWon)
                .Select((x, i) => new ScoreRow { PlayerId = x.PlayerId, GamesWon = x.GamesWon, Rank = i + 1 })
                .ToArray();

        public class ScoreRow
        {
            public int Rank { get; set; }
            public string PlayerId { get; set; }
            public int GamesWon { get; set; }
            public int RoundsWon { get; set; }
            public int GamesPlayed { get; set; }
            public int RoundsPlayed { get; set; }
        }
    }
}
