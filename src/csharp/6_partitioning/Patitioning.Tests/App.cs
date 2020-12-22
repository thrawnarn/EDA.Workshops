using System;
using System.Collections.Generic;
using System.Text;

namespace Patitioning.Tests
{
    public struct RoomDamageReported : IRoomEvent
    {
        public string RoomId { get; }
        public string DamageDescription { get; }
        public string SubjectId => RoomId;

        public RoomDamageReported(string roomId, string damageDescription)
        {
            RoomId = roomId;
            DamageDescription = damageDescription;
        }
    }

    public struct RoomCleaningRequested : IRoomEvent
    {
        public string RoomId { get; }
        public string SubjectId => RoomId;

        public RoomCleaningRequested(string roomId)
        {
            RoomId = roomId;
        }
    }

    public struct RoomCleaned : IRoomEvent
    {
        public RoomCleaned(string roomId)
        {
            RoomId = roomId;
        }
        public string SubjectId => RoomId;

        public string RoomId { get; }
    }

    public struct RoomCheckedIn : IRoomEvent
    {
        public RoomCheckedIn(string roomId)
        {
            RoomId = roomId;
        }

        public string SubjectId => RoomId;

        public string RoomId { get; }
    }

    public struct RoomCheckedAsOk : IRoomEvent
    {
        public string RoomId { get; }

        public string SubjectId => RoomId;

        public RoomCheckedAsOk(string roomId)
        {
            RoomId = roomId;
        }
    }

    public struct GuestCheckedOut : IRoomEvent
    {
        public string RoomId { get; }

        public string SubjectId => RoomId;

        public GuestCheckedOut(string roomId)
        {
            RoomId = roomId;
        }
    }

    public interface IRoomEvent : IEvent
    {
        string RoomId { get; }
    }

    public interface IEvent
    {
        string SubjectId { get; }
    }


}
