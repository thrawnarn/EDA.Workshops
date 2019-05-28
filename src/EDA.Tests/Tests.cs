using System;
using System.Linq;
using Xunit;

namespace EDA.Tests
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            //ACT
            // (c, state) -> events
            var happened = Bribe.Handle(new Plan { BribeId = Guid.NewGuid(), InitialValue = 100 }, new BribeState());

            //ASSERT
            Assert.True(happened.OfType<Planned>().Any());
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
}
