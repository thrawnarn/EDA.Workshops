# Instructions

## Exercise #2

Get all the tests in "[GameStateChangeTests](RPS.Tests/GameStateChangeTests.cs)" green by;

1. Implementing the methods in "[Game](RPS.Tests/Game.cs)". 

2. Adding overloads with matching events to the "[gamestate](RPS.Tests/GameState.cs)" class.

        public GameState When(IEvent @event) => this;

*Note* - don't remove/replace/rename the When(IEvent).

```bash
dotnet watch test
```
All test uses an extensions to "replay" the history into a state, using;

```csharp
Rehydrate<GameState>();
```

Take a look how the When functions are used in [Extensions](RPS.Tests/Extensions.cs).

Bonus;
- The the test "gameend" requiers a bit more implementation, remove the "skip" property and give it a try.
- The test doens use the "RoundTied" event, investigate how this might affect your solution.

