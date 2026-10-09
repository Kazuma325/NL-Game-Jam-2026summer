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

            eventBus.Subscribe<LoopStartedEvent>(
                OnLoopStarted);

            eventBus.Subscribe<KnowledgeSlotSelectionChangedEvent>(
                OnKnowledgeSlotSelectionChanged);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<ItemCraftedEvent>(
                OnItemCrafted);

            eventBus.Unsubscribe<LoopStartedEvent>(
                OnLoopStarted);

            eventBus.Unsubscribe<KnowledgeSlotSelectionChangedEvent>(
                OnKnowledgeSlotSelectionChanged);
        }

        private void OnItemCrafted(
            ItemCraftedEvent eventData)
        {
            soundManager.PlaySe(
                SeId.BlenderCreate);
        }

        private void OnLoopStarted(
            LoopStartedEvent eventData)
        {
            if (eventData.LoopCount <= 1)
            {
                return;
            }

            soundManager.PlaySe(
                SeId.LoopRestart);
        }

        private void OnKnowledgeSlotSelectionChanged(
    KnowledgeSlotSelectionChangedEvent eventData)
        {
            soundManager.PlaySe(SeId.KnowledgeSelect);
        }
    }
}