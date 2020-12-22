type GameCreated = { gameId: string; playerId: string; title: string; rounds: number; created: Date; sourceId: string }
type GameStarted = { readonly gameId: string; playerId: string; sourceId: string }
type RoundStarted = { readonly gameId: string; round: number; sourceId: string }
type GameEvent = { type: "gameCreated"; event: GameCreated } | { type: "gameStarted"; event: GameStarted } | { type: "roundStarted"; event: RoundStarted }
type GameState = { status: GameStatus }

enum GameStatus { None = 0, ReadyToStart = 10, Started = 20, Ended = 50 }

export const apply = (state: GameState, e: GameEvent): GameState => {
    switch (e.type) {
        case 'gameCreated':
            return { ...state, status: GameStatus.ReadyToStart }
        default:
            return state;
    }
}

