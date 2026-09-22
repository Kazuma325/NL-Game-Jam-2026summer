using System;
using ShopGame.Core.Bootstrap;
using UnityEngine;

namespace ShopGame.Adventure.View
{
    public class DoorViewController : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private DoorView doorView;

        private void Start()
        {
            if (gameBootstrap == null) throw new InvalidOperationException("GameBootstrap is not assigned.");
            if (doorView == null) throw new InvalidOperationException("DoorView is not assigned.");

            doorView.Validate();
        }

        public void OnInteract()
        {
            gameBootstrap.Context.Navigation.MoveThroughConnection(doorView.ConnectionId);

            Debug.Log(gameBootstrap.Context.World.CurrentRoomId);
        }
    }
}
