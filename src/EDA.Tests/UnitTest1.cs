using System;
using System.Linq;
using Xunit;

namespace EDA.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //ACT
            // (c, state) -> events
            var happend = Bribe.Handle(new Plan { BribeId = Guid.NewGuid(), InitialValue = 100 }, new BribeState());

            //ASSERT
            Assert.True(happend.OfType<Planed>().Any());
        }

        [Fact]
        public void Test2()
        {
            //ARRANGE
            var state = new BribeState();
            
            //ACT
            // (c, state) -> events
            var happend = Bribe.Handle(new Plan { BribeId = Guid.NewGuid(), InitialValue = 100 }, state);

            //ASSERT
            // (events, state) -> newState
            var newState = happend.Aggregate(state, (s, e) => s.When(e));

            Assert.Equal(100, newState.Value);
        }
    }

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

    public class Planed : IEvent
    {
        public Planed(Guid bribeId)
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
