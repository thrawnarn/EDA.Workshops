using System;
using System.Collections.Generic;
using System.Text;

namespace EDA.Tests
{

    public static class Bribe
    {
        public static IEvent[] Handle(Plan command, BribeState state)
         => Array.Empty<IEvent>();
    }

    public class Plan
    {
        public Guid BribeId { get; set; }

        public int InitialValue { get; set; }
    }

    public class Planned : IEvent
    {
        public Planned(Guid bribeId)
        {
            BribeId = bribeId;
        }

        public string SourceId => BribeId.ToString();
        public Guid BribeId { get; }
    }

    public class BribeState
    {
        public Guid Id { get; private set; }

        public int Value { get; private set; }

        public BribeStatus Status { get; private set; }

        public BribeState When(IEvent @event) => Apply((dynamic)@event);

        BribeState Apply(IEvent @event) => this;

    }

    public enum BribeStatus
    {
        Plotting = 0,
        Offered = 10,
        Accepted = 20,
        Rejected = 30
    }

    public interface IEvent
    {
        string SourceId { get; }
    }
}
