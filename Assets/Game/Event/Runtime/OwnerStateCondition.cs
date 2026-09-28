using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class OwnerStateCondition : ICondition
    {
        private readonly bool expectedPresence;

        public OwnerStateCondition(bool expectedPresence)
        {
            this.expectedPresence = expectedPresence;
        }

        public bool Evaluate(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.World.Owner.IsPresent == expectedPresence;
        }
    }
}