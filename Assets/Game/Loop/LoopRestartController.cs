using System;
using ShopGame.Core.EventBus;

namespace ShopGame.Loop
{
    public sealed class LoopRestartController
    {
        private readonly LoopManager loopManager;
        private readonly EventBus eventBus;

        private bool restartPending;

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

        public void Update()
        {
            if (!restartPending)
                return;

            restartPending = false;
            loopManager.RestartLoop();
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<LoopEndedEvent>(
                OnLoopEnded);
        }

        private void OnLoopEnded(
    LoopEndedEvent eventData)
        {
            restartPending = true;
        }
    }
}