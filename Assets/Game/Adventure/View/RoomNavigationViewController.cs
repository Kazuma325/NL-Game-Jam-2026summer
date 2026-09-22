using System;
using ShopGame.Core.Bootstrap;
using ShopGame.Core.EventBus;
using UnityEngine;

namespace ShopGame.Adventure.View
{
    public sealed class RoomNavigationViewController : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private GameObject leftArrow;

        [SerializeField]
        private GameObject rightArrow;

        [SerializeField]
        private GameObject backArrow;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (gameBootstrap == null) throw new InvalidOperationException("GameBootstrap is not assigned.");
            if (leftArrow == null) throw new InvalidOperationException("Left arrow is not assigned.");
            if (rightArrow == null) throw new InvalidOperationException("Right arrow is not assigned.");
            if (backArrow == null) throw new InvalidOperationException("Back arrow is not assigned.");

            gameBootstrap.Context.EventBus.Subscribe<RoomChangedEvent>(OnRoomChanged);

            RefreshNavigation();
        }

        public void OnClickLeft()
        {
            gameBootstrap.Context.Navigation.MoveLeft();
        }

        public void OnClickRight()
        {
            gameBootstrap.Context.Navigation.MoveRight();
        }

        public void OnClickBack()
        {
            gameBootstrap.Context.Navigation.MoveBack();
        }

        private void OnRoomChanged(RoomChangedEvent eventData)
        {
            RefreshNavigation();
        }

        private void RefreshNavigation()
        {
            string currentRoomId = gameBootstrap.Context.World.CurrentRoomId;

            var roomData = gameBootstrap.Context.Rooms.CurrentRoomdata;

            bool hasLeftConnection = false;
            bool hasRightConnection = false;
            bool hasBackConnection = false;

            foreach (var connection in roomData.Connections)
            {
                if(connection == null) continue;

                switch (connection.Direction)
                {
                    case Data.RoomConnectionDirection.Left:
                        hasLeftConnection = true;
                        break;

                    case Data.RoomConnectionDirection.Right:
                        hasRightConnection = true;
                        break;

                    case Data.RoomConnectionDirection.Back:
                        hasBackConnection = true;
                        break;
                }
            }

            leftArrow.SetActive(hasLeftConnection);
            rightArrow.SetActive(hasRightConnection);
            backArrow.SetActive(hasBackConnection);
        }
    }
}