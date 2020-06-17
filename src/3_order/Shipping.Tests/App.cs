using System;
using System.Collections.Generic;
using System.Text;

namespace Shipping.Tests
{
    public class Ship : ICommand
    { }

    public class GoodsShipped : IEvent
    {
        public string SourceId => "shipping.order";

        public IDictionary<string, string> Meta { get; set; }
    }

    public class PaymentRecieved : IEvent
    {
        public string SourceId => "payment.order";
        public IDictionary<string, string> Meta { get; set; }
    }

    public class GoodsPicked : IEvent
    {
        public string SourceId => "warehouse.order";

        public IDictionary<string, string> Meta { get; set; }
    }

    public interface ICommand
    {}

    public interface IEvent
    {
        string SourceId { get; }
        IDictionary<string, string> Meta { get; }
    }
}
