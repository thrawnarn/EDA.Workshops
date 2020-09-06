using System;
using System.Collections.Generic;

namespace Shipping.Tests
{
    public class App
    {
        List<IEvent> history = new List<IEvent>();

        public void Given(params IEvent[] events) => history.AddRange(events);

        public void When(IEvent @event)
        {
            var cmd = ShippingPolicy.When((dynamic)@event);
            var state = history.Rehydrate<Order>();
            history.AddRange(OrderBehavior.Handle(state, (dynamic)cmd));
        }

        public void Then(Action<IEvent[]> f)
            => f(history.ToArray());
    }


    public class ShippingPolicy
    {
        public static ICommand When(PaymentRecieved @event) => new CompletePayment();
        public static ICommand When(GoodsPicked @event) => new CompletePacking();
    }

    public static class OrderBehavior
    {
        public static IEnumerable<IEvent> Handle(this Order order, CompletePayment command)
            => new[] { new PaymentComplete() };

        public static IEnumerable<IEvent> Handle(this Order order, CompletePacking command)
            => new[] { new PackingComplete() };
    }

    public class Order
    {
        public bool Payed;
        public bool Packed;

        public Order When(IEvent @event) => this;

        public Order When(PaymentRecieved @event) => this;
        public Order When(GoodsPicked @event) => this;

    }
}
