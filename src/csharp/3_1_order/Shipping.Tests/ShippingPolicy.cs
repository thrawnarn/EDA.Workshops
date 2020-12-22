using System;
using System.Collections.Generic;
using System.Text;

namespace Shipping.Tests
{
    public class ShippingPolicy
    {
        public static ICommand When(PaymentRecieved @event, Order state) => Ship(state);
        public static ICommand When(GoodsPicked @event, Order state) => Ship(state);

        private static ICommand Ship(Order state)
           => null;
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
