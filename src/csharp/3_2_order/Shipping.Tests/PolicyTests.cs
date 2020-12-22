using System;
using System.Linq;
using Xunit;

namespace Shipping.Tests
{
    public class PolicyTests
    {
        [Fact]
        public void PickedDoesntShip()
        {
            var app = new App();

            //Given
            app.Given(Array.Empty<IEvent>());

            //When
            app.When(new GoodsPicked());

            //Then
            app.Then(events => Assert.False(events.OfType<GoodsShipped>().Any()));
        }

        [Fact]
        public void PayedAndPickedShip()
        {
            var app = new App();

            //Given
            app.Given(new PaymentRecieved());

            //When
            app.When(new GoodsPicked());

            //Then
            app.Then(events => Assert.True(events.OfType<GoodsShipped>().Any()));
        }

        [Fact]
        public void PickedAndPayedIssueShip()
        {
            var app = new App();

            //Given
            app.Given(new GoodsPicked());

            //When
            app.When(new PaymentRecieved());

            //Then
            app.Then(events => Assert.True(events.OfType<GoodsShipped>().Any()));
         }
    }
}
