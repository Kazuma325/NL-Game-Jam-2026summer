using System;
using System.Collections.Generic;
using ShopGame.Core.EventBus;

namespace ShopGame.State.Progress
{
    public sealed class GameProgress
    {
        private readonly EventBus eventBus;
        private readonly HashSet<string> completedProgressIds = new();

        public GameProgress(EventBus eventBus)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public bool HasProgress(string progressId)
        {
            ValidateProgressId(progressId);
            return completedProgressIds.Contains(progressId);
        }

        public bool CompleteProgress(string progressId)
        {
            ValidateProgressId(progressId);
            if (!completedProgressIds.Add(progressId))
            {
                return false;
            }

            eventBus.Publish(new ProgressCompletedEvent(progressId));
            return true;
        }

        public IReadOnlyCollection<string> GetAllProgress()
        {
            return completedProgressIds;
        }

        private static void ValidateProgressId(string progressId)
        {
            if (string.IsNullOrWhiteSpace(progressId))
            {
                throw new ArgumentException("Progress ID must not be null, empty, or whitespace.", nameof(progressId));
            }
        }
    }
}
