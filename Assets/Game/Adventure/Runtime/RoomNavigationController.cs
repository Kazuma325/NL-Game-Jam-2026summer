using System;
using UnityEngine;

namespace ShopGame.Adventure.Runtime
{
    public sealed class RoomNavigationController
    {
        private readonly RoomManager roomManager;

        public RoomNavigationController(RoomManager roomManager)
        {
            this.roomManager = roomManager ?? throw new ArgumentNullException(nameof(roomManager));
        }

        public bool MoveLeft()
        {
            return roomManager.TryMoveThroughConnection(Data.RoomConnectionDirection.Left);
        }

        public bool MoveRight()
        {
            return roomManager.TryMoveThroughConnection(Data.RoomConnectionDirection.Right);
        }

        public bool MoveBack()
        {
            return roomManager.TryMoveThroughConnection(Data.RoomConnectionDirection.Back);
        }

        public bool MoveThroughConnection(string connectionId)
        {
            return roomManager.TryMoveThroughConnection(connectionId);
        }
    }
}
