using System;
using System.Collections.Generic;
using System.Linq;

namespace RPS.Tests
{
    public class HighScoreView
    {
        public HighScoreView When(IEvent @event) => this;

        public ScoreRow[] Rows { get; set; } = Array.Empty<ScoreRow>();

        readonly Dictionary<Guid, List<(string player, int score)>> Games = new Dictionary<Guid, List<(string player, int score)>>();

        public HighScoreView When(RoundEnded @event)
        {
            if (!Games.ContainsKey(@event.GameId))
                Games.Add(@event.GameId, new List<(string player, int score)> { (@event.Looser, 0), (@event.Winner, 1) });
            else
            {
                var game = Games[@event.GameId].Single(x => x.player == @event.Winner);
                game.score++;
            }

            if (!Rows.Any(x => x.PlayerId == @event.Looser))
                Rows = Rows.Append(new ScoreRow { PlayerId = @event.Looser }).ToArray();

            if (!Rows.Any(x => x.PlayerId == @event.Winner))
                Rows = Rows.Append(new ScoreRow { PlayerId = @event.Winner }).ToArray();

            return this;
        }

        public HighScoreView When(GameEnded @event)
        {
            var (player, score) = Games[@event.GameId]
                .OrderByDescending(x => x.score)
                .First();

            var r = Rows.Single(x => x.PlayerId == player);
            r.GamesPlayed++;
            r.GamesWon++;

            Games.Remove(@event.GameId);
            Rows = Rows.OrderByDescending(x => x.GamesWon).Select((x, i) =>
            {
                x.Rank = i + 1;
                return x;
            }).ToArray();
            return this;
        }

        public HighScoreView When(GamePlayed @event)
        {
            if (!Rows.Any(x => x.PlayerId == @event.Looser))
                Rows = Rows.Append(new ScoreRow { PlayerId = @event.Looser }).ToArray();
            if (!Rows.Any(x => x.PlayerId == @event.Winner))
                Rows = Rows.Append(new ScoreRow { PlayerId = @event.Winner }).ToArray();

            Rows = Rows
                .Select(x =>
                {
                    if (x.PlayerId == @event.Winner)
                        x.GamesWon++;
                    if (x.PlayerId == @event.Looser || x.PlayerId == @event.Winner)
                    {
                        x.GamesPlayed++;
                        x.RoundsPlayed = x.RoundsPlayed + @event.Rounds;
                    }
                    return x;
                })
                .OrderByDescending(x => x.GamesWon)
                .Select((x, i) =>
                {
                    x.Rank = i + 1;
                    return x;
                })
                .ToArray();

            return this;
        }


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
