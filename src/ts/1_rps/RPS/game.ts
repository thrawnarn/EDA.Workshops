type GameCreated = { readonly gameId: string; readonly playerId: string; readonly title: string; readonly rounds: number; readonly created: Date; sourceId: string }
type GameStarted = { readonly gameId: string; playerId: string; sourceId: string }
type RoundStarted = { readonly gameId: string; round: number; sourceId: string }
type GameEvent = { type: "gameCreated"; event: GameCreated } | { type: "gameStarted"; event: GameStarted } | { type: "roundStarted"; event: RoundStarted }
type GameState = { status: GameStatus }

enum GameStatus { None = 0, ReadyToStart = 10, Started = 20, Ended = 50 }

function apply(state: GameState, e: GameEvent): GameState {
    switch (e.type) {
        case 'gameCreated':
            return { ...state, status: GameStatus.ReadyToStart }
        case 'gameStarted':
            return { ...state, status: GameStatus.Started }
        default:
            return state;
    }
}

export { apply, GameCreated, GameStarted, RoundStarted, GameEvent, GameStatus };

