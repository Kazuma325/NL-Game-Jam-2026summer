using System;
using System.Collections.Generic;

namespace ShopGame.Event.Runtime
{
    public sealed class Rule
    {
        public string RuleId { get; }
        public InteractionTrigger Trigger { get; }
        public ICondition Condition { get; }
        public int Priority { get; }
        public IReadOnlyList<IEffect> Effects { get; }

        public Rule(
            string ruleId,
            InteractionTrigger trigger,
            ICondition condition,
            int priority,
            IReadOnlyList<IEffect> effects)
        {
            if (string.IsNullOrWhiteSpace(ruleId))
            {
                throw new ArgumentException(
                    "Rule ID must not be null, empty, or whitespace.",
                    nameof(ruleId));
            }

            Trigger = trigger
                ?? throw new ArgumentNullException(nameof(trigger));

            Condition = condition
                ?? throw new ArgumentNullException(nameof(condition));

            if (effects == null)
            {
                throw new ArgumentNullException(nameof(effects));
            }

            if (effects.Count == 0)
            {
                throw new ArgumentException(
                    "Rule must contain at least one effect.",
                    nameof(effects));
            }

            RuleId = ruleId;
            Priority = priority;
            Effects = effects;
        }
    }
}