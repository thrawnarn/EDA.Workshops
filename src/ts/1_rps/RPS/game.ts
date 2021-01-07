type GameCreated = { readonly gameId: string; readonly playerId: string; readonly title: string; readonly rounds: number; readonly created: Date; sourceId: string }
type GameStarted = { readonly gameId: string; readonly playerId: string; readonly sourceId: string }
type RoundStarted = { readonly gameId: string; readonly round: number; readonly sourceId: string }
type HandShown = { readonly gameId: string; readonly hand: Hand; readonly playerId: string; readonly sourceId: string }
type RoundEnded = { readonly gameId: string; readonly round: number; readonly looser: string; readonly winner: string; readonly sourceId: string }
type GameEnded = { readonly gameId: string; readonly sourceId: string }

enum Hand { None = 0, Rock = 10, Paper = 20, Scissors = 30 }

type GameEvent =
    { type: "gameCreated"; event: GameCreated } |
    { type: "gameStarted"; event: GameStarted } |
    { type: "roundStarted"; event: RoundStarted } |
    { type: "handShown"; event: HandShown } |
    { type: "roundEnded"; event: RoundEnded } |
    { type: "gameEnded"; event: GameEnded }

type GameState = { status: GameStatus }

enum GameStatus { None = 0, ReadyToStart = 10, Started = 20, Ended = 50 }

function apply(state: GameState, e: GameEvent): GameState {
    switch (e.type) {
        case 'gameCreated':
            return { ...state, status: GameStatus.ReadyToStart }
        case 'gameStarted':
            return { ...state, status: GameStatus.Started }
        case 'gameEnded':
            return { ...state, status: GameStatus.Ended }
        default:
            return state;
    }
}

export { apply, GameCreated, GameStarted, RoundStarted, HandShown, RoundEnded, GameEnded, GameEvent, GameStatus, Hand };

