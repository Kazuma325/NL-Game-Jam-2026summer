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
        private RuleTriggerController ruleTriggerController;

        public void Initialize(GameContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            gameContext = context;

            ruleTriggerController =
                context.RuleTrigger;

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
            InteractionTrigger trigger =
                new InteractionTrigger(
                    "owner");

            ruleTriggerController.TryTrigger(
                trigger,
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