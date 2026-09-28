using System;
using System.Collections.Generic;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleResolver
    {
        private readonly IReadOnlyList<Rule> rules;

        public RuleResolver(IReadOnlyList<Rule> rules)
        {
            this.rules = rules
                ?? throw new ArgumentNullException(nameof(rules));
        }

        public Rule Resolve(
            InteractionTrigger trigger,
            GameContext context)
        {
            if (trigger == null)
                throw new ArgumentNullException(nameof(trigger));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Rule bestMatch = null;

            foreach (Rule rule in rules)
            {
                if (rule == null)
                    continue;

                if (rule.Trigger.TargetId != trigger.TargetId)
                    continue;

                if (!rule.Condition.Evaluate(context))
                    continue;

                if (bestMatch == null ||
                    rule.Priority > bestMatch.Priority)
                {
                    bestMatch = rule;
                }
            }

            return bestMatch;
        }
    }
}