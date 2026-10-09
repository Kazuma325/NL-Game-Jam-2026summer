using ShopGame.Core.Context;
using ShopGame.Core.EventBus;
using ShopGame.Event.Runtime;
using System;

namespace ShopGame.Interaction.Runtime
{
    public sealed class CraftResultHandler
    {
        private readonly EventBus eventBus;
        private readonly RecipeMatcher recipeMatcher;
        private readonly RuleTriggerController ruleTriggerController;
        private readonly GameContext gameContext;

        public CraftResultHandler(
    EventBus eventBus,
    RecipeMatcher recipeMatcher,
    RuleTriggerController ruleTriggerController,
    GameContext gameContext)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            this.recipeMatcher =
                recipeMatcher
                ?? throw new ArgumentNullException(
                    nameof(recipeMatcher));

            this.ruleTriggerController =
                ruleTriggerController
                ?? throw new ArgumentNullException(
                    nameof(ruleTriggerController));

            this.gameContext =
                gameContext
                ?? throw new ArgumentNullException(
                    nameof(gameContext));

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

                CraftingTrigger trigger =
                    new CraftingTrigger(
                        itemId);

                ruleTriggerController.TryTrigger(
                    trigger,
                    gameContext);

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