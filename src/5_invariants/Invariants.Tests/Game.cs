using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invariants.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(JoinGame command, GameState state)
        {
            if (state.Players.PlayerOne.Id == command.PlayerId)
                yield break;

            if (state.Players.PlayerTwo == default)
            {
                yield return new GameStarted { GameId = command.GameId, PlayerId = command.PlayerId };
                yield return new RoundStarted { GameId = command.GameId, Round = 1 };
            }
        }

        public static IEnumerable<IEvent> Handle(JoinGame command, IEvent[] events)
        {
            if (events
                .OfType<GameCreated>()
                .Any(e => e.PlayerId == command.PlayerId))
                yield break;

            if (!events
                .OfType<GameStarted>()
                .Any(e => e.PlayerId == command.PlayerId))
            {
                yield return new GameStarted { GameId = command.GameId, PlayerId = command.PlayerId };
                yield return new RoundStarted { GameId = command.GameId, Round = 1 };
            }
        }

    }
}
