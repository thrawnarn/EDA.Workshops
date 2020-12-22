namespace RPS.Tests
{
    public class GameState
    {
        public GameState When(IEvent @event) => this;

        public enum GameStatus
        {
            None = 0,
            ReadyToStart = 10,
            Started = 20,
            Ended = 50
        }
    }

}
