import * as game from './game';
import * as highscore from './highscore';


function gameEvents(gameId: string, title: string, loosingPlayer: string, winningPlayer: string): game.GameEvent[] {
    const created: game.GameCreated = { gameId: gameId, sourceId: gameId, playerId: loosingPlayer, title: title, rounds: 2, created: new Date() }
    const gameStarted: game.GameStarted = { gameId: gameId, sourceId: gameId, playerId: winningPlayer }
    const roundStarted: game.RoundStarted = { gameId: gameId, sourceId: gameId, round: 1 }
    const handShown1: game.HandShown = { gameId: gameId, sourceId: gameId, hand: game.Hand.Scissors, playerId: loosingPlayer }
    const handShown2: game.HandShown = { gameId: gameId, sourceId: gameId, hand: game.Hand.Rock, playerId: winningPlayer }
    const roundEnded: game.RoundEnded = { gameId: gameId, sourceId: gameId, round: 1, looser: loosingPlayer, winner: winningPlayer }
    const roundStarted2: game.RoundStarted = { gameId: gameId, sourceId: gameId, round: 2 }
    const handShown21: game.HandShown = { gameId: gameId, sourceId: gameId, hand: game.Hand.Paper, playerId: loosingPlayer }
    const handShown22: game.HandShown = { gameId: gameId, sourceId: gameId, hand: game.Hand.Scissors, playerId: winningPlayer }
    const roundEnded2: game.RoundEnded = { gameId: gameId, sourceId: gameId, round: 2, looser: loosingPlayer, winner: winningPlayer }
    const gameEnded: game.GameEnded = { gameId: gameId, sourceId: gameId }
    const rc: game.GameEvent[] =
    [
        { type: 'gameCreated', event: created },
        { type: 'gameStarted', event: gameStarted },
        { type: 'roundStarted', event: roundStarted },
        { type: 'handShown', event: handShown1 },
        { type: 'handShown', event: handShown2 },
        { type: 'roundEnded', event: roundEnded },
        { type: 'roundStarted', event: roundStarted2 },
        { type: 'handShown', event: handShown21 },
        { type: 'handShown', event: handShown22 },
        { type: 'roundEnded', event: roundEnded2 },
        { type: 'gameEnded', event: gameEnded }
        ];
    return rc;
}

test('highscore', () => {
    const playerOne = "alex@rpsgame.com";
    const playerTwo = "lisa@rpsgame.com";
    const playerThree = "julie@rpsgame.com";

    //given
    const events = [...Array(20)].map((value, index) => gameEvents(index.toString(), 'game_' + index, playerOne, playerTwo))
    .concat([...Array(10)].map((value, index) => gameEvents((index + 20).toString(), 'game_' + index + 20, playerTwo, playerOne)))
    .concat([...Array(15)].map((value, index) => gameEvents((index + 30).toString(), 'game_' + index + 30, playerThree, playerOne)))
    .concat([...Array(5)].map((value, index) => gameEvents((index + 45).toString(), 'game_' + index + 45, playerTwo, playerThree)))
    .reduce((l, r) => l.concat(r), [])

    const highscoreView = { games: new Map<string, highscore.score[]>(), rows: new Array<highscore.scoreRow>() }

    //when
    const newState = events.reduce(highscore.apply, highscoreView)

    //then
    //Assert.Equal(25, state.Rows.OrderBy(r => r.Rank).First().GamesWon);
    //Assert.Equal(playerOne, state.Rows.OrderBy(r => r.Rank).First().PlayerId);
    console.dir(newState.rows);
    expect(newState.rows.sort(x => x.gamesWon)[1].gamesWon).toBe(25);
    expect(newState.games.size).toBe(50);
    expect(newState.rows.map(s => s.gamesWon).reduce((l, r) => l + r)).toBe(50);
});




