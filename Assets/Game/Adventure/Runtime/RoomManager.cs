using System;
using System.Collections.Generic;
using ShopGame.Adventure.Data;
using GameEventBus = ShopGame.Core.EventBus.EventBus;
using ShopGame.Core.EventBus;
using ShopGame.State.World;

namespace ShopGame.Adventure.Runtime
{
    public sealed class RoomManager
    {
        private readonly WorldState worldState;
        private readonly GameEventBus eventBus;
        private readonly Dictionary<string, RoomData> roomsById;
        private readonly RoomAccessChecker roomAccessChecker;

        public string CurrentRoomId => worldState.CurrentRoomId;
        public RoomData CurrentRoomdata => GetCurrentRoomData();

        public RoomManager(
            WorldState worldState,
            GameEventBus eventBus,
            IReadOnlyCollection<RoomData> roomData,
            RoomAccessChecker roomAccessChecker)
        {
            this.worldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            if (roomData == null)
            {
                throw new ArgumentNullException(nameof(roomData));
            }

            roomsById = new Dictionary<string, RoomData>();
            foreach (RoomData room in roomData)
            {
                if (room == null)
                {
                    throw new ArgumentException("Room data collection must not contain null entries.", nameof(roomData));
                }

                ValidateRoomId(room.RoomId, nameof(roomData));
                if (roomsById.ContainsKey(room.RoomId))
                {
                    throw new ArgumentException($"Duplicate room ID: {room.RoomId}", nameof(roomData));
                }

                roomsById.Add(room.RoomId, room);
            }

            GetRoomDataOrThrow(worldState.CurrentRoomId);
            this.roomAccessChecker = roomAccessChecker ?? throw new ArgumentNullException(nameof(roomAccessChecker));
        }

        public RoomConnectionData GetConnection(string connectionId)
        {
            ValidateConnectionId(connectionId, nameof(connectionId));

            RoomData currentRoom = GetCurrentRoomData();

            foreach (RoomConnectionData connection in currentRoom.Connections)
            {
                if (connection == null) continue;
                if(connection.ConnectionId == connectionId) return connection;
            }

            throw new InvalidOperationException($"Connection was not found in current room. " + $"Room: {CurrentRoomId}, Connection: {connectionId}");
        }

        public RoomConnectionData GetConnection(RoomConnectionDirection direction)
        {
            RoomData currentRoom = GetCurrentRoomData();

            foreach (RoomConnectionData connection in currentRoom.Connections)
            {
                if(connection == null) continue;

                if(connection.Direction == direction) return connection;
            }

            return null;
        }

        public bool ChangeRoom(string roomId)
        {
            ValidateRoomId(roomId, nameof(roomId));
            GetRoomDataOrThrow(roomId);

            string previousRoomId = CurrentRoomId;
            if (previousRoomId == roomId)
            {
                return false;
            }

            worldState.SetCurrentRoom(roomId);
            eventBus.Publish(new RoomChangedEvent(previousRoomId, roomId));
            return true;
        }

        public bool TryMoveThroughConnection(string connectionId)
        {
            RoomConnectionData connection = GetConnection(connectionId);

            if(!roomAccessChecker.CanAccess(connection)) return false;

            return ChangeRoom(connection.TargetRoomId);
        }

        public bool TryMoveThroughConnection(RoomConnectionDirection direction)
        {
            RoomConnectionData connection = GetConnection(direction);

            if (connection == null) return false;

            return TryMoveThroughConnection(connection.ConnectionId);
        }

        private RoomData GetCurrentRoomData()
        {
            return GetRoomDataOrThrow(CurrentRoomId);
        }

        private RoomData GetRoomDataOrThrow(string roomId)
        {
            if (!roomsById.TryGetValue(roomId, out RoomData roomData))
            {
                throw new InvalidOperationException($"Room data was not found for room ID: {roomId}");
            }

            return roomData;
        }

        private static void ValidateRoomId(string roomId, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                throw new ArgumentException("Room ID must not be null, empty, or whitespace.", parameterName);
            }
        }

        private static void ValidateConnectionId(string connectionId, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(connectionId)) throw new ArgumentException("Connection ID must be null, empty, or whitespace.", parameterName);
        }
    }
}
