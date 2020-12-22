import * as game from './game';

test('created', () => {
    const e: game.GameCreated = { gameId: 'test', sourceId: 'test', playerId: '', title: '', rounds: 1, created: new Date() }
    const env: game.GameEvent = { type: 'gameCreated', event: e }
    const state = { status: game.GameStatus.None }

    const newState = game.apply(state, env);
    expect(newState.status).toBe(game.GameStatus.ReadyToStart);
});


test('started', () => {

    //given
    const created: game.GameCreated = { gameId: 'test', sourceId: 'test', playerId: '', title: '', rounds: 1, created: new Date() }
    const gameStarted: game.GameStarted = { gameId: 'test', sourceId: 'test', playerId: '' }
    const roundStarted: game.RoundStarted = { gameId: 'test', sourceId: 'test', round : 1}

    const events: game.GameEvent[] =
        [
            { type: 'gameCreated', event: created },
            { type: 'gameStarted', event: gameStarted },
            { type: 'roundStarted', event: roundStarted}
        ]
    const state = { status: game.GameStatus.None }

    //when
    const newState = events.reduce(game.apply, state)

    //then
    expect(newState.status).toBe(game.GameStatus.Started);
});