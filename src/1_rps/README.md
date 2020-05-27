# Instructions

## Exercise #1-1

Get all the tests in "[GameStateViewTests](RPS.Tests/GameStateViewTests.cs)" green by adding overloads with matching events to the "[gamestate](RPS.Tests/GameState.cs)" class.

        public GameState When(IEvent @event) => this;

*Note* - don't remove/replace/rename the When(IEvent).

```bash
dotnet watch test
```

## Exercise #1-2

Get all the tests in "[HighScoreViewTests](RPS.Tests/HighScoreViewTests.cs)" green by adding overload to "[HighScoreView](RPS.Tests/HighScoreView.cs)"

## Excercise #1-3

Get all tests in "[HighScoreViewTestsIntegration](RPS.Tests/HighScoreViewTestsIntegration.cs)" green by adding overloads to "GamePlayed" and in later test add an overload to "HighScoreView"

