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

Take a look how the When functions are used in [Extensions](RPS.Tests/Extensions.cs).


## Exercise #1-2

Get all the tests in "[HighScoreViewTests](RPS.Tests/HighScoreViewTests.cs)" green by adding overload to "[HighScoreView](RPS.Tests/HighScoreView.cs)"
Note that a full implementation is not needed. Focus on getting the test to pass.

## Excercise #1-3

Get all tests in "[HighScoreViewTestsIntegration](RPS.Tests/HighScoreViewTestsIntegration.cs)" green by adding overloads to "GamePlayed" and in later test add an overload to "HighScoreView"
Note that a full implementation is not needed. Focus on getting the test to pass.
Bonus;
- one part of the view is impossible to implement, why ?
- And how could this be fixed?

