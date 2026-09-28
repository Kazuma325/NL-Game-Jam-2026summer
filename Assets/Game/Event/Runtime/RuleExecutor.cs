using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleExecutor
    {
        public bool Execute(
            Rule rule,
            GameContext context)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            foreach (IEffect effect in rule.Effects)
            {
                if (effect == null)
                    throw new InvalidOperationException(
                        $"Rule '{rule.RuleId}' contains a null effect.");

                bool succeeded = effect.Execute(context);

                if (!succeeded)
                {
                    return false;
                }
            }

            return true;
        }
    }
}