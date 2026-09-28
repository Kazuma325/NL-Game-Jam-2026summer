using ShopGame.Core.Context;
using ShopGame.Event.Runtime;
using ShopGame.Interaction.Runtime;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Interaction.View
{
    public sealed class OwnerInteractionViewController : MonoBehaviour
    {
        [SerializeField]
        private Button interactionButton;

        private GameContext gameContext;
        private RuleInteractionController interactionController;

        public void Initialize(GameContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            gameContext = context;

            interactionController =
                context.RuleInteraction;

            if (interactionButton == null)
            {
                throw new InvalidOperationException(
                    "Interaction Button is not assigned.");
            }

            interactionButton.onClick.AddListener(
                HandleInteractionClicked);
        }

        private void HandleInteractionClicked()
        {
            interactionController.TryInteract(
                "owner",
                gameContext);
        }

        private void OnDestroy()
        {
            if (interactionButton == null)
            {
                return;
            }

            interactionButton.onClick.RemoveListener(
                HandleInteractionClicked);
        }
    }
}