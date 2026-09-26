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

        private KnowledgeInteractionController knowledgeInteractionController;
        private CraftingStation craftingStation;

        public void Initialize(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            knowledgeInteractionController =
                context.KnowledgeInteraction;

            craftingStation =
                context.CraftingStation;

            interactionButton.onClick.AddListener(
                HandleInteractionClicked);
        }

        private void HandleInteractionClicked()
        {
            KnowledgeUseResult result =
                knowledgeInteractionController
                    .TryUseKnowledgeOnCraftingStation(
                        craftingStation);

            Debug.Log(
                $"CraftingStation interaction result: {result}");
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