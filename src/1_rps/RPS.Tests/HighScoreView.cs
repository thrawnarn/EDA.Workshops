using System;
using System.Collections.Generic;
using System.Linq;

namespace RPS.Tests
{
    public class HighScoreView
    {
        private class GameState
        {
            public string Title { get; set; }
            public int Rounds { get; set; }
            public int CurrentRound { get; set; }
            public string Player1 { get; set; }
            public string Player2 { get; set; }
            public int Player1Wins { get; set; }
            public int Player2Wins { get; set; }
        }

        Dictionary<Guid, GameState> states = new Dictionary<Guid, GameState>();

        public HighScoreView When(GameCreated @event)
        {
            states.Add(@event.GameId, new GameState()
            {
                Player1 = @event.PlayerId,
                Rounds = @event.Rounds,
                Title = @event.Title
            });

            return this;
        }

        public HighScoreView When(GameStarted @event)
        {
            states[@event.GameId].Player2 = @event.PlayerId;
            return this;
        }

        public HighScoreView When(RoundStarted @event)
        {
            var state = states[@event.GameId];
            state.CurrentRound = @event.Round;
            return this;
        }

        public HighScoreView When(HandShown @event)
        {
            //no neeed right now
            return this;
        }

        public HighScoreView When(RoundEnded @event)
        {
            var state = states[@event.GameId];
            var rows = GetWinnerLooser(@event.Winner, @event.Looser);
            rows.winnerRow.RoundsPlayed++;
            rows.winnerRow.RoundsWon++;
            rows.looserRow.RoundsPlayed++;

            if (@event.Winner.Equals(state.Player1))
                state.Player1Wins++;
            else
                state.Player2Wins++;

            return this;
        }

        public HighScoreView When(GameEnded @event)
        {
            var state = states[@event.GameId];
            var winner = state.Player1Wins > state.Player2Wins ? state.Player1 : state.Player2;
            var looser = state.Player1Wins < state.Player2Wins ? state.Player1 : state.Player2;
            var rows = GetWinnerLooser(winner, looser);

            rows.winnerRow.GamesPlayed++;
            rows.winnerRow.GamesWon++;
            rows.looserRow.GamesPlayed++;

            states.Remove(@event.GameId);


            var winners = Rows.OrderByDescending(p => p.GamesWon).ToList();
            for (var i = 0; i < winners.Count; i++)
                winners[i].Rank = i + 1;

            return this;
        }

        public HighScoreView When(GamePlayed @event)
        {
            var rows = GetWinnerLooser(@event.Winner, @event.Looser);
            
            rows.winnerRow.GamesPlayed++;
            rows.winnerRow.GamesWon++;
            rows.winnerRow.RoundsPlayed += @event.Rounds;
            rows.looserRow.GamesPlayed++;
            rows.looserRow.RoundsPlayed += @event.Rounds;

            states.Remove(@event.GameId);


            var winners = Rows.OrderByDescending(p => p.GamesWon).ToList();
            for (var i = 0; i < winners.Count; i++)
                winners[i].Rank = i + 1;

            return this;
        }

        private (ScoreRow winnerRow, ScoreRow looserRow) GetWinnerLooser(string winner, string looser)
        {
            if(Rows == null)
                Rows = new List<ScoreRow>();
            var winnerRow = Rows.FirstOrDefault(p => p.PlayerId == winner);
            if (winnerRow == null)
                Rows.Add((winnerRow = new ScoreRow()
                {
                    GamesPlayed = 0,
                    GamesWon = 0,
                    PlayerId = winner,
                    Rank = 0,
                    RoundsPlayed = 0,
                    RoundsWon = 0
                }));
            var looserRow = Rows.FirstOrDefault(p => p.PlayerId == looser);
            if (looserRow == null)
                Rows.Add((looserRow = new ScoreRow()
                {
                    GamesPlayed = 0,
                    GamesWon = 0,
                    PlayerId = looser,
                    Rank = 0,
                    RoundsPlayed = 0,
                    RoundsWon = 0
                }));
            return (winnerRow, looserRow);
        }


        public HighScoreView When(IEvent @event) => this;
        public List<ScoreRow> Rows { get; set; }
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
