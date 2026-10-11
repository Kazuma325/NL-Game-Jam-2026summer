using System;
using ShopGame.Core.EventBus;

namespace ShopGame.Sound.Runtime
{
    public sealed class SeController
    {
        private readonly SoundManager soundManager;
        private readonly EventBus eventBus;

        public SeController(
            SoundManager soundManager,
            EventBus eventBus)
        {
            this.soundManager =
                soundManager
                ?? throw new ArgumentNullException(nameof(soundManager));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            eventBus.Subscribe<ItemCraftedEvent>(
                OnItemCrafted);

            eventBus.Subscribe<KnowledgeSlotSelectionChangedEvent>(
                OnKnowledgeSlotSelectionChanged);

            eventBus.Subscribe<InteractionPerformedEvent>(
                OnInteractionPerformed);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<ItemCraftedEvent>(
                OnItemCrafted);

            eventBus.Unsubscribe<KnowledgeSlotSelectionChangedEvent>(
                OnKnowledgeSlotSelectionChanged);

            eventBus.Unsubscribe<InteractionPerformedEvent>(
                OnInteractionPerformed);
        }

        private void OnItemCrafted(
            ItemCraftedEvent eventData)
        {
            soundManager.PlaySe(
                SeId.BlenderCreate);
        }

        private void OnKnowledgeSlotSelectionChanged(
    KnowledgeSlotSelectionChangedEvent eventData)
        {
            soundManager.PlaySe(SeId.KnowledgeSelect);
        }

        private void OnInteractionPerformed(
    InteractionPerformedEvent eventData)
        {
            soundManager.PlaySe(SeId.Interaction);
        }

        public void PlayLoopRestartSe()
        {
            soundManager.PlaySe(SeId.LoopRestart);
        }
    }
}