namespace RPS.Tests
{
    public class GameState
    {
        public GameState When(IEvent @event) => this;
        public GameState When(GameCreated @event) 
        {
            Status = GameStatus.ReadyToStart;
            return this;
        }

        public GameState When(GameStarted @event)
        {
            Status = GameStatus.Started;
            return this;
        }

        public GameState When(GameEnded @event)
        {
            Status = GameStatus.Ended;
            return this;
        }

        public GameStatus Status { get; set; }
    }

}
