using System;

namespace ShopGame.Event.Runtime
{
    public sealed class InteractionTrigger
    {
        public string TargetId { get; }

        public InteractionTrigger(string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId))
                throw new ArgumentException(
                    "Target ID must not be null, empty, or whitespace.",
                    nameof(targetId));

            TargetId = targetId;
        }
    }
}