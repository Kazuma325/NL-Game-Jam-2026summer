using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class CustomerStateCondition : ICondition
    {
        private readonly bool expectedPresence;

        public CustomerStateCondition(bool expectedPresence)
        {
            this.expectedPresence = expectedPresence;
        }

        public bool Evaluate(GameContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.World.Customers.IsPresent == expectedPresence;
        }
    }
}