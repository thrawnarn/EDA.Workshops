using System;
using System.Linq;
using Xunit;

namespace Patitioning.Tests
{
    public class RoomTests
    {
        [Fact]
        public void CheckedIn()
        {
            //GIVEN
            var store = new EventStore();

            var history = new IEvent []
            {
                new RoomCleaned("101"),
                new RoomCleaned("102"),
                new RoomCleaned("103"),
                new RoomCheckedIn("102")
            };

            store.Append(history);

            //Then
            var checkedin = store.GetCheckedInRoomIds();

            Assert.True(checkedin.Count() == 1);
            Assert.True(checkedin.First() == "102");
        }

        [Fact]
        public void CleaningRequest()
        {
            //GIVEN
            var store = new EventStore();

            var history = new IEvent[]
            {
                new RoomCleaned("101"),
                new RoomCleaned("102"),
                new RoomCleaned("103"),
                new RoomCleaningRequested("205"),
                new RoomCheckedIn("102")
            };

            store.Append(history);

            //Then
            var checkedin = store.GetRoomsToClean();

            Assert.True(checkedin.Count() == 1);
            Assert.True(checkedin.First() == "205");
        }
    }
}
