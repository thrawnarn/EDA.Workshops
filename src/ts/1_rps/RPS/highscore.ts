import * as game from './game';

type highScoreView = { readonly games: Map<string, score[]>; readonly rows: scoreRow[] }
type score = { readonly playerId: string; readonly score: number }
type scoreRow = { readonly rank: number; readonly playerId: string; readonly gamesWon: number; readonly roundsWon: number; readonly gamesPlayed: number; readonly roundsPlayed: number }

function apply(state: highScoreView, e: game.GameEvent): highScoreView {
    switch (e.type) {
        case 'roundEnded': {
            let rc: highScoreView = state;

            if (!rc.rows.some(x => x.playerId === e.event.looser))
                rc = { ...rc, rows: rc.rows.concat([{ rank: 0, playerId: e.event.looser, gamesWon: 0, gamesPlayed: 0, roundsPlayed: 1, roundsWon: 0 }]) }

            if (!rc.rows.some(x => x.playerId === e.event.winner))
                rc = { ...rc, rows: rc.rows.concat([{ rank: 0, playerId: e.event.winner, gamesWon: 0, gamesPlayed: 0, roundsPlayed: 1, roundsWon: 1 }]) }

            if (!rc.games.has(e.event.gameId)) {
                return { ...rc, games: rc.games.set(e.event.gameId, [{ playerId: e.event.looser, score: 0 }, { playerId: e.event.winner, score: 1 }]) }
            }
            else {
                const s = rc.games.get(e.event.gameId);
                const looser = s.find(x => x.playerId === e.event.looser);
                let winner = s.find(x => x.playerId === e.event.winner);
                winner = { ...winner, score: +winner.score };
                return { ...rc, games: rc.games.set(e.event.gameId, [looser, winner]) }
            }
        }
        case 'gameEnded': {
            const winner = state.games.get(e.event.gameId).sort(x => x.score)[1];
            const index = state.rows.findIndex(x => x.playerId === winner.playerId);
            const newArr = [...state.rows.slice(0, index), { ...state.rows[index], gamesWon: state.rows[index].gamesWon + 1, }, ...state.rows.slice(index + 1)]
            return { ...state, rows: newArr}
        }
        default:
            return state;
    }
}

export { apply, highScoreView, score, scoreRow };

