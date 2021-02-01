using System;
using System.Linq;

namespace Patitioning.Tests
{
    public static class EventStoreRoomExtensions
    { 
            public static string[] GetCheckedInRoomIds(this EventStore store)
            => store.Read(0, int.MaxValue)
                .Select(x => x.Content)
				.OfType<IRoomEvent>()
				.GroupBy(o => o.RoomId)
				.Where(o => o.Last() is RoomCheckedIn)
				.Select(o => o.Key)
				.ToArray();

            public static string[] GetRoomsToClean(this EventStore store)
                => store.Read(0, int.MaxValue)
                    .Select(p => p.Content)
                    .OfType<IRoomEvent>()
                    .GroupBy(p => p.RoomId)
                    .Where(p => p.Last() is RoomCleaningRequested)
                    .Select(p => p.Key)
                    .ToArray();

    }
}
