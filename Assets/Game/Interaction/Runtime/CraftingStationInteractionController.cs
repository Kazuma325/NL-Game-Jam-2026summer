using System;
using ShopGame.Core.Context;
using ShopGame.Dialogue.Runtime;

namespace ShopGame.Interaction.Runtime
{
    public sealed class CraftingStationInteractionController
    {
        private readonly KnowledgeInteractionController knowledgeInteractionController;
        private readonly DialogueInteractionController dialogueInteractionController;
        private readonly CraftingStation craftingStation;

        public CraftingStationInteractionController(
            KnowledgeInteractionController knowledgeInteractionController,
            DialogueInteractionController dialogueInteractionController,
            CraftingStation craftingStation)
        {
            this.knowledgeInteractionController =
                knowledgeInteractionController
                ?? throw new ArgumentNullException(
                    nameof(knowledgeInteractionController));

            this.dialogueInteractionController =
                dialogueInteractionController
                ?? throw new ArgumentNullException(
                    nameof(dialogueInteractionController));

            this.craftingStation =
                craftingStation
                ?? throw new ArgumentNullException(
                    nameof(craftingStation));
        }

        public void Interact(
            string targetId,
            GameContext gameContext)
        {
            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Target ID must not be null, empty, or whitespace.",
                    nameof(targetId));
            }

            if (gameContext == null)
                throw new ArgumentNullException(nameof(gameContext));

            if (gameContext.KnowledgeSlot.HasSelection)
            {
                knowledgeInteractionController
                    .TryUseKnowledgeOnCraftingStation(
                        craftingStation);

                return;
            }

            DialogueResolutionResult resolutionResult =
                dialogueInteractionController.ResolveInteraction(
                    targetId,
                    gameContext);

            if (resolutionResult == null)
                return;

            gameContext.DialogueManager.StartDialogue(
                resolutionResult);
        }
    }
}