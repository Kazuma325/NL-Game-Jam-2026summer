using System;
using ShopGame.Core.EventBus;
using ShopGame.State.World;

namespace ShopGame.Loop
{
    public sealed class LoopManager
    {
        private readonly WorldState worldState;
        private readonly EventBus eventBus;

        public int LoopCount { get; private set; }

        public bool IsLoopActive { get; private set; }

        public LoopManager(WorldState worldState, EventBus eventBus)
        {
            this.worldState = worldState ?? throw new ArgumentNullException(nameof(worldState));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void StartLoop()
        {
            if (IsLoopActive)
            {
                return;
            }

            LoopCount++;
            IsLoopActive = true;
            eventBus.Publish(new LoopStartedEvent(LoopCount));
        }

        public void EndLoop()
        {
            if (!IsLoopActive)
            {
                return;
            }

            IsLoopActive = false;

            eventBus.Publish(new LoopEndedEvent(LoopCount));
        }

        public void RestartLoop()
        {
            if (IsLoopActive)
            {
                EndLoop();
            }

            worldState.Reset();
            StartLoop();
        }
    }
}
