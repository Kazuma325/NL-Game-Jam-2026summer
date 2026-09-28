using System;
using ShopGame.Core.Context;
using ShopGame.Core.EventBus;
using ShopGame.Interaction.Runtime;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class CraftingDialogueTrigger
    {
        private readonly GameContext gameContext;
        private readonly RecipeMatcher recipeMatcher;
        private readonly string targetId;

        public CraftingDialogueTrigger(
            GameContext gameContext,
            RecipeMatcher recipeMatcher,
            string targetId)
        {
            this.gameContext =
                gameContext
                ?? throw new ArgumentNullException(
                    nameof(gameContext));

            this.recipeMatcher =
                recipeMatcher
                ?? throw new ArgumentNullException(
                    nameof(recipeMatcher));

            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Target ID must not be null, empty, or whitespace.",
                    nameof(targetId));
            }

            this.targetId = targetId;
        }

        public void Initialize()
        {
            gameContext.EventBus.Subscribe<ItemCraftedEvent>(
                OnItemCrafted);
        }

        public void Dispose()
        {
            gameContext.EventBus.Unsubscribe<ItemCraftedEvent>(
                OnItemCrafted);
        }

        private void OnItemCrafted(
            ItemCraftedEvent eventData)
        {
            recipeMatcher.TryMatch(
                eventData.MaterialIds,
                out string itemId);

            var context =
                new DialogueInteractionContext(
                    targetId,
                    itemId,
                    gameContext);

            DialogueResolutionResult resolutionResult =
                gameContext.DialogueResolver.Resolve(
                    context);

            if (resolutionResult == null)
                return;

            gameContext.DialogueManager.StartDialogue(
                resolutionResult);
        }
    }
}