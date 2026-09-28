using System;
using ShopGame.Core.EventBus;

namespace ShopGame.State.Progress
{
    public sealed class GameProgress
    {
        private readonly EventBus eventBus;

        public int CurrentProgress { get; private set; }

        public GameProgress(EventBus eventBus)
        {
            this.eventBus = eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            CurrentProgress = 0;
        }

        public bool AdvanceTo(int targetProgress)
        {
            if (targetProgress < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(targetProgress),
                    "Progress must not be negative.");
            }

            if (targetProgress <= CurrentProgress)
            {
                return false;
            }

            CurrentProgress = targetProgress;

            eventBus.Publish(
                new ProgressCompletedEvent(
                    CurrentProgress.ToString()));

            return true;
        }

        public void Reset()
        {
            CurrentProgress = 0;
        }
    }
}