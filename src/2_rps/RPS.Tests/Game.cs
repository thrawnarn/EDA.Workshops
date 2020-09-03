using System;
using System.Collections.Generic;
using System.Text;
using static RPS.Tests.GameState;

namespace RPS.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(CreateGame command, GameState state)
              => new[] {
                    new GameCreated
                    {
                               GameId = command.GameId,
                               PlayerId = command.PlayerId,
                               Title = command.Title,
                               Rounds = command.Rounds,
                               Created = DateTime.UtcNow,
                               Status = GameStatus.ReadyToStart
                    }
            };

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

        public static IEnumerable<IEvent> Handle(PlayGame command, GameState state)
        {
            if (state.Status != GameStatus.Started)
                yield break;

            if (command.Hand == Hand.None) 
                yield break;

            var players = state.Players switch
            {
                { } p when command.PlayerId == p.PlayerOne.Id => (Active: state.Players.PlayerOne, Passive: state.Players.PlayerTwo), { } p when command.PlayerId == p.Item1.Id => (Active: state.Players.Item1, Passive: state.Players.Item2),
                { } p when command.PlayerId == p.PlayerTwo.Id => (Active: state.Players.PlayerTwo, Passive: state.Players.PlayerOne),
                _ => throw new ArgumentException("Player not in game")
            };

            yield return players.Active.Hand switch
            {
                Hand.None => new HandShown { GameId = command.GameId, PlayerId = players.Active.Id, Hand = command.Hand },
                _ => throw new ArgumentException("Changing hand not allowed")
            };

            var endRound = players.Passive.Hand != Hand.None;

            if (!endRound)
                yield break;


            var activePlayerResult = (command.Hand, players.Passive.Hand) switch
            {
                (Hand.Paper, Hand.Paper) => RoundResult.Tied,
                (Hand.Paper, Hand.Rock) => RoundResult.Won,
                (Hand.Paper, Hand.Scissors) => RoundResult.Lost,
                (Hand.Rock, Hand.Paper) => RoundResult.Lost,
                (Hand.Rock, Hand.Rock) => RoundResult.Tied,
                (Hand.Rock, Hand.Scissors) => RoundResult.Won,
                (Hand.Scissors, Hand.Paper) => RoundResult.Won,
                (Hand.Scissors, Hand.Rock) => RoundResult.Lost,
                (Hand.Scissors, Hand.Scissors) => RoundResult.Tied,
                _ => RoundResult.Tied
            };

            yield return activePlayerResult switch
            {
                RoundResult.Won => new RoundEnded { GameId = command.GameId, Winner = players.Active.Id, Looser = players.Passive.Id, Round = state.Round },
                RoundResult.Lost => new RoundEnded { GameId = command.GameId, Winner = players.Passive.Id, Looser = players.Active.Id, Round = state.Round },
                _ => new RoundTied { GameId = command.GameId, Round = state.Round },
            };

            yield return (state.Rounds == state.Round) switch
            {
                true => new GameEnded { GameId = command.GameId },
                _ => new RoundStarted { GameId = command.GameId, Round = state.Round + 1 }
            };
        }


    }
}
