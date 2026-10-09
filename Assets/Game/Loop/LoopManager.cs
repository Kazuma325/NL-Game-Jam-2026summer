using ShopGame.Core.EventBus;
using ShopGame.State.World;
using System;
using System.Collections.Generic;

namespace ShopGame.Loop
{
    public sealed class LoopManager
    {
        private readonly IReadOnlyList<ILoopResettable> resettableStates;
        private readonly EventBus eventBus;

        public int LoopCount { get; private set; }

        public bool IsLoopActive { get; private set; }

        public LoopManager(IReadOnlyList<ILoopResettable> resettableStates, EventBus eventBus)
        {
            this.resettableStates = resettableStates ?? throw new ArgumentNullException(nameof(resettableStates));
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

            foreach (ILoopResettable resettable in resettableStates)
            {
                resettable.ResetForLoop();
            }

            StartLoop();
        }
    }
}
