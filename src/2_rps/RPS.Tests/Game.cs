using System;
using System.Collections.Generic;
using System.Text;

namespace RPS.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(CreateGame command, GameState state)
         => Array.Empty<IEvent>();

        public static IEnumerable<IEvent> Handle(JoinGame command, GameState state)
         => Array.Empty<IEvent>();


    }
}
