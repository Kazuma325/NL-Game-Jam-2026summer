using System;
using ShopGame.Core.Context;
using ShopGame.Event.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Event.View
{
    public sealed class RuleInteractionView : MonoBehaviour
    {
        [SerializeField]
        private Button interactionButton;

        [SerializeField]
        private string targetId;

        private GameContext gameContext;
        private RuleTriggerController ruleTriggerController;

        public void Initialize(GameContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            if (interactionButton == null)
            {
                throw new InvalidOperationException(
                    "Interaction Button is not assigned.");
            }

            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new InvalidOperationException(
                    "Target ID must not be null, empty, or whitespace.");
            }

            gameContext = context;
            ruleTriggerController =
                context.RuleTrigger;

            interactionButton.onClick.AddListener(
                HandleInteractionClicked);
        }

        private void HandleInteractionClicked()
        {
            InteractionTrigger trigger =
                new InteractionTrigger(
                    targetId);

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