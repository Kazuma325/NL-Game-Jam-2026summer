using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class AdvanceProgressEffect : IEffect
    {
        private readonly int targetProgress;

        public AdvanceProgressEffect(int targetProgress)
        {
            if (targetProgress < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(targetProgress),
                    "Target progress must not be negative.");
            }

            this.targetProgress = targetProgress;
        }

        public bool Execute(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Progress.AdvanceTo(targetProgress);
            return true;
        }
    }
}