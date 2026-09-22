using System;

namespace ShopGame.State.World
{
    public sealed class WorldState
    {
        private readonly string initialRoomId;

        public string CurrentRoomId { get; private set; }

        public BombState Bomb { get; }

        public CustomerState Customers { get; }

        public EventState Events { get; }

        public WorldState(string initialRoomId)
        {
            ValidateRoomId(initialRoomId);

            this.initialRoomId = initialRoomId;
            CurrentRoomId = initialRoomId;
            Bomb = new BombState();
            Customers = new CustomerState();
            Events = new EventState();
        }

        public void SetCurrentRoom(string roomId)
        {
            ValidateRoomId(roomId);
            CurrentRoomId = roomId;
        }

        public void Reset()
        {
            CurrentRoomId = initialRoomId;
            Bomb.Reset();
            Customers.Reset();
            Events.Reset();
        }

        private static void ValidateRoomId(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                throw new ArgumentException("Room ID must not be null, empty, or whitespace.", nameof(roomId));
            }
        }
    }
}
