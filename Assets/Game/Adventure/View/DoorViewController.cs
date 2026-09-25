using System;
using ShopGame.Adventure.Runtime;
using ShopGame.Core.Bootstrap;
using ShopGame.Interaction.Runtime;
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
            var context = gameBootstrap.Context;
            var connection = context.Rooms.GetConnection(doorView.ConnectionId);

            if (string.IsNullOrEmpty(connection.RequiredKnowledgeId)) return;

            var door = new Door(connection.ConnectionId, connection.RequiredKnowledgeId);

            KnowledgeUseResult result = context.KnowledgeInteraction.TryUseKnowledge(door, () => context.Navigation.MoveThroughConnection(door.ConnectionId));

            Debug.Log($"Door interaction result: {result}");
        }
    }
}
