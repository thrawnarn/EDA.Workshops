namespace RPS.Tests
{
    public class HighScoreView
    {
        public HighScoreView When(IEvent @event) => this;
        public ScoreRow[] Rows { get; set; }
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
