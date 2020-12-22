# Instructions

## Exercise #3-2

Get all the tests in "[PolicyTests](Shipping.Tests/PolicyTests.cs)" green by;

1. Implementing the methods in "[ShippingPolicy](Shipping.Tests/ShippingPolicy.cs)". 

2. Implement both rehyration of Order(state) and the Behavior of the order aggregate. But thats not all there is something in the infrastructure that needs fixing. 

```bash
dotnet watch test
```
All test uses an extensions to "replay" the history into a state, using;

```csharp
Rehydrate<GameState>();
```

Take a look how the When functions are used in [Extensions](RPS.Tests/Extensions.cs).


