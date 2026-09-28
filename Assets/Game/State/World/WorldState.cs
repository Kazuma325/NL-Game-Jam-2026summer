using System;
using ShopGame.Core.EventBus;

namespace ShopGame.State.World
{
    public sealed class WorldState
    {
        private readonly string initialRoomId;
        private readonly EventBus eventBus;

        public string CurrentRoomId { get; private set; }

        public BombState Bomb { get; }

        public CustomerState Customers { get; }

        public EventState Events { get; }

        public OwnerState Owner { get; }

        public WorldState(string initialRoomId, EventBus eventBus)
        {
            ValidateRoomId(initialRoomId);

            this.initialRoomId = initialRoomId;
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            CurrentRoomId = initialRoomId;
            Bomb = new BombState();
            Customers = new CustomerState(eventBus);
            Events = new EventState();
            Owner = new OwnerState(eventBus);
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
            Owner.Reset();
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
