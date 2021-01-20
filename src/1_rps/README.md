# Instructions

## Exercise #1-1

Get all the tests in "[GameStateViewTests](RPS.Tests/GameStateViewTests.cs)" green by adding overloads with matching events to the "[gamestate](RPS.Tests/GameState.cs)" class.

        public GameState When(IEvent @event) => this;

*Note* - don't remove/replace/rename the When(IEvent).

```bash
dotnet watch test
```
All test uses an extensions to "replay" the history into a state, using;

```csharp
Rehydrate<GameState>();
```

Take a look how the *When* functions are used in [Extensions](RPS.Tests/Extensions.cs).

### .net core 3.1 - C# dynamic

The Rehydrate extensions hides a way fold/aggregate/reduce. When using state classes, a convention to have an *When* methods to create a new state for each event.
There needs to always be a matching overload (IEvent as guard).

```csharp
public static TState Rehydrate<TState>(this IEnumerable<IEvent> events) where TState : new()
   => events.Aggregate(new TState(), (s, @event) => ((dynamic)s).When((dynamic)@event));
```

### .net 5.0 C# records

Moving towards immutable events, we could use records. Records could also fulfill the *When* convention.  

```csharp
public static TState Apply<TState>(this IEnumerable<EventRecord> events, TState currentState) where TState : new()
 => events.Aggregate(currentState, (s, @event) => ((dynamic)s).When((@event)));
```
But to be more functional we could remove the *When* convention and use aggregate with an apply function.

```csharp
public static TState Apply<TState>(this IEnumerable<EventRecord> events, TState currentState, Func<TState, EventRecord, TState> apply) where TState : new()
         => events.Aggregate(currentState, apply);
```

```csharp
var newState = events.Aggregate(currentState, apply);
```

Using the *When* convention with a record might look like this;


```csharp
public GameState When(EventRecord @event) =>
        @event switch
        {
                GameCreated e => this with
                {
                Status = GameStatus.ReadyToStart,
                Id = e.GameId,
                Players = (new(e.PlayerId, default), new(string.Empty, default)),
                Rounds = e.Rounds
                },
                GameStarted e => this with
                {
                Status = GameStatus.Started,
                Players = (Players.PlayerOne, new(e.PlayerId, default))
                },
                ...
                ...
                _ => this
        }
```

Moving to use a passed apply function in the same case could look like this;

```csharp
public static GameState Apply(GameState current, EventRecord @event) =>
        @event switch
        {
                GameCreated e => current with
                {
                Status = GameStatus.ReadyToStart,
                Id = e.GameId,
                Players = (new(e.PlayerId, default), new(string.Empty, default)),
                Rounds = e.Rounds
                },
                GameStarted e => current with
                {
                Status = GameStatus.Started,
                Players = (Players.PlayerOne, new(e.PlayerId, default))
                },
                ...
                ...
                _ => this
        }
```

### Typescript/js

In javascript we use reduce in the same way.

```typescript
const newState = events.reduce(game.apply, state)
```

### F#

In F# we use (left) fold.

```fsharp
let newState = List.fold apply state events 
```

## Exercise #1-2

Get all the tests in "[HighScoreViewTests](RPS.Tests/HighScoreViewTests.cs)" green by adding overload to "[HighScoreView](RPS.Tests/HighScoreView.cs)"
Note that a full implementation is not needed. Focus on getting the test to pass.

## Excercise #1-3

Get all tests in "[HighScoreViewTestsIntegration](RPS.Tests/HighScoreViewTestsIntegration.cs)" green by adding overloads to "GamePlayed" and in later test add an overload to "HighScoreView"
Note that a full implementation is not needed. Focus on getting the test to pass.
Bonus;
- one part of the view is impossible to implement, why ?
- And how could this be fixed?

