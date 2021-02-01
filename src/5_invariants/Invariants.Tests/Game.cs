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
            if(state.Players.Any(p=>p.Id.Equals(command.PlayerId)))
                yield break;

            yield return new GameStarted { GameId = command.GameId, PlayerId = command.PlayerId };
            yield return new RoundStarted { GameId = command.GameId, Round = 1 };
        }

        public static IEnumerable<IEvent> Handle(JoinGame command, IEvent[] events)
        {
            if (events.OfType<GameCreated>().Any(p => p.PlayerId.Equals(command.PlayerId)))
                yield break;
            if (!events.OfType<GameCreated>().Any())
                yield break;

            yield return new GameStarted { GameId = command.GameId, PlayerId = command.PlayerId };
            yield return new RoundStarted { GameId = command.GameId, Round = 1 };
        }

    }
}
