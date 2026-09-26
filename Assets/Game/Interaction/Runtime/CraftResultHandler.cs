using System;
using ShopGame.Core.EventBus;

namespace ShopGame.Interaction.Runtime
{
    public sealed class CraftResultHandler
    {
        private readonly EventBus eventBus;
        private readonly RecipeMatcher recipeMatcher;

        public CraftResultHandler(
            EventBus eventBus,
            RecipeMatcher recipeMatcher)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            this.recipeMatcher =
                recipeMatcher
                ?? throw new ArgumentNullException(
                    nameof(recipeMatcher));

            eventBus.Subscribe<ItemCraftedEvent>(
                HandleItemCrafted);
        }

        private void HandleItemCrafted(
            ItemCraftedEvent eventData)
        {
            bool matched =
                recipeMatcher.TryMatch(
                    eventData.MaterialIds,
                    out string itemId);

            if (matched)
            {
                UnityEngine.Debug.Log(
                    $"Craft result matched: {itemId}");

                return;
            }

            UnityEngine.Debug.Log(
                "Craft result has no recognized recipe.");
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<ItemCraftedEvent>(
                HandleItemCrafted);
        }
    }
}