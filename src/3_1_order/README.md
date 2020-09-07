# Instructions

## Exercise #3-1

Get all the tests in "[PolicyTests](Shipping.Tests/PolicyTests.cs)" green by;

1. Implementing the methods in "[ShippingPolicy](Shipping.Tests/ShippingPolicy.cs)". 

2. There is a Ship method that now is hard to use. Get the tests passing, then reflect on what we could say about the events relation to the state. 
Could you change the test itself to reflect the the relation between state and event, that lets you use the Ship method.

```bash
dotnet watch test
```
All test uses an extensions to "replay" the history into a state, using;

```csharp
Rehydrate<GameState>();
```

Take a look how the When functions are used in [Extensions](RPS.Tests/Extensions.cs).


