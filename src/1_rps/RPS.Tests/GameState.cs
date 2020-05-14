namespace RPS.Tests
{
    public class GameState
    {
        public GameState When(IEvent @event) => this;
        public GameStatus Status { get; set; }
    }

}
