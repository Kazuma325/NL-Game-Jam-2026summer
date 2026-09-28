using System;
using ShopGame.Core.Context;
using ShopGame.Interaction.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Interaction.View
{
    public sealed class CraftingStationInteractionViewController : MonoBehaviour
    {
        [SerializeField]
        private Button interactionButton;

        private GameContext gameContext;
        private CraftingStationInteractionController interactionController;

        public void Initialize(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            gameContext = context;

            interactionController =
                context.CraftingStationInteraction;

            interactionButton.onClick.AddListener(
                HandleInteractionClicked);
        }

        private void HandleInteractionClicked()
        {
            interactionController.Interact(
                "blender",
                gameContext);
        }

        private void OnDestroy()
        {
            if (interactionButton == null)
                return;

            interactionButton.onClick.RemoveListener(
                HandleInteractionClicked);
        }
    }
}