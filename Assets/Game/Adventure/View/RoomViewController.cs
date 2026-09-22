using System;
using ShopGame.Core.Bootstrap;
using ShopGame.Core.EventBus;
using UnityEngine;

namespace ShopGame.Adventure.View
{
    public sealed class RoomViewController : MonoBehaviour
    {
        [Serializable]
        private sealed class RoomPanelBinding
        {
            [SerializeField]
            private string roomId;

            [SerializeField]
            private GameObject roomPanel;

            public string RoomId => roomId;
            public GameObject RoomPanel => roomPanel;
        }

        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private RoomView roomView;

        [SerializeField]
        private RoomPanelBinding[] roomPanels = Array.Empty<RoomPanelBinding>();

        private void Start()
        {
            if (gameBootstrap == null) throw new InvalidOperationException("GameBootstrap is not assigned.");
            if (roomView == null) throw new InvalidOperationException("RoomView is not assigned.");

            ValidateRoomPanels();

            gameBootstrap.Context.EventBus.Subscribe<RoomChangedEvent>(OnRoomChanged);
            DisplayCurrentRoom();
        }

        private void OnDestroy()
        {
            if (gameBootstrap == null || gameBootstrap.Context == null) return;

            gameBootstrap.Context.EventBus.Unsubscribe<RoomChangedEvent>(OnRoomChanged);
        }

        private void OnRoomChanged(RoomChangedEvent eventData)
        {
            DisplayCurrentRoom();
        }

        private void DisplayCurrentRoom()
        {
            string currentRoomId = gameBootstrap.Context.Rooms.CurrentRoomId;

            roomView.DisplayRoom(gameBootstrap.Context.Rooms.CurrentRoomdata);

            foreach (RoomPanelBinding binding in roomPanels)
            {
                if(binding == null || binding.RoomPanel == null) continue;

                binding.RoomPanel.SetActive(binding.RoomId == currentRoomId);
            }
        }

        private void ValidateRoomPanels()
        {
            foreach (RoomPanelBinding binding in roomPanels)
            {
                if(binding == null) continue;

                if(string.IsNullOrEmpty(binding.RoomId)) throw new InvalidOperationException("Room panel binding has an empty Room ID.");
                if (binding.RoomPanel == null) throw new InvalidOperationException($"Room panel is not assigned for room: " + $"{binding.RoomId}");
            }
        }
    }
}
