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

        [Theory]
        [InlineData(100, 100)]
        [InlineData(200, 200)]
        public void Test2(int value, int expected)
        {
            //ARRANGE
            var state = new BribeState();
            
            //ACT
            // (c, state) -> events
            var happend = Bribe.Handle(new Plan { BribeId = Guid.NewGuid(), InitialValue = value }, state);

            //ASSERT
            // (events, state) -> newState
            var newState = happend.Aggregate(state, (s, e) => s.When(e));

            Assert.Equal(expected, newState.Value);
        }

        [Fact]
        public void Test3()
        {
            //ARRANGE
            var state = new BribeState();
            var id = Guid.NewGuid();

            //ACT
            // (c, state) -> events
            var happend = Bribe.Handle(new Plan { BribeId = id, InitialValue = value }, state);

            //ASSERT
            // (events, state) -> newState
            var newState = happend.Aggregate(state, (s, e) => s.When(e));

            Assert.Equal(id, newState.Id);
        }
    }
}
