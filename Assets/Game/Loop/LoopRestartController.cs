using System;
using ShopGame.Core.EventBus;

namespace ShopGame.Loop
{
    public sealed class LoopRestartController
    {
        private readonly LoopManager loopManager;
        private readonly EventBus eventBus;

        public LoopRestartController(
            LoopManager loopManager,
            EventBus eventBus)
        {
            this.loopManager =
                loopManager
                ?? throw new ArgumentNullException(
                    nameof(loopManager));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            eventBus.Subscribe<LoopEndedEvent>(
                OnLoopEnded);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<LoopEndedEvent>(
                OnLoopEnded);
        }

        private void OnLoopEnded(
            LoopEndedEvent eventData)
        {
            loopManager.RestartLoop();
        }
    }
}