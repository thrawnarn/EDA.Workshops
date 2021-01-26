using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPS.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(CreateGame command, GameState state)
        {
            yield return new GameCreated()
            {
                Created = DateTime.Now,
                GameId = command.GameId,
                PlayerId = command.PlayerId,
                Rounds = command.Rounds,
                Title = command.Title,
                Status = GameState.GameStatus.ReadyToStart
            };
        }

        public static IEnumerable<IEvent> Handle(JoinGame command, GameState state)
        {
            if (!state.Players.Any(p => p.Id.Equals(command.PlayerId)))
            {
                yield return new GameStarted()
                {
                    GameId = command.GameId,
                    PlayerId = command.PlayerId
                };
                yield return new RoundStarted()
                {
                    GameId = command.GameId,
                };
            }
        }

        public static IEnumerable<IEvent> Handle(PlayGame command, GameState state)
        {
            if (state.Status != GameState.GameStatus.Started)
                yield break;

            if (command.Hand == Hand.None)
                yield break;

            var player = state.Players.SingleOrDefault(p => p.Id.Equals(command.PlayerId));
            if(player == null)
                throw new Exception("Player not in game");
            if(player.Hand != Hand.None)
                throw new Exception("Player already show hand");

            if (state.Players.Count(p => p.Hand == Hand.None) == 1 && player.Hand == Hand.None)
            {
                var otherPlayer = state.Players.Single(p => p != player); //ugly for now!
                var playerResult = (command.Hand, otherPlayer.Hand) switch
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

                yield return playerResult switch
                {
                    RoundResult.Won => new RoundEnded { GameId = command.GameId, Winner = player.Id, Looser = otherPlayer.Id, Round = state.Round },
                    RoundResult.Lost => new RoundEnded { GameId = command.GameId, Winner = otherPlayer.Id, Looser = player.Id, Round = state.Round },
                    _ => new RoundTied { GameId = command.GameId, Round = state.Round },
                };

                yield return (state.Rounds == state.Round) switch
                {
                    true => new GameEnded { GameId = command.GameId },
                    _ => new RoundStarted { GameId = command.GameId, Round = state.Round + 1 }
                };
            }
            else
            {
                yield return new HandShown()
                {
                    GameId = command.GameId,
                    PlayerId = command.PlayerId,
                    Hand = command.Hand
                };
            }


        }
    }
}
