using System;
using System.Collections.Generic;

namespace ShopGame.Event.Data
{
    public sealed class RuleData
    {
        public string RuleId { get; }
        public string TargetId { get; }
        public int Priority { get; }
        public ConditionData Condition { get; }
        public IReadOnlyList<EffectData> Effects { get; }

        public RuleData(
            string ruleId,
            string targetId,
            int priority,
            ConditionData condition,
            IReadOnlyList<EffectData> effects)
        {
            if (string.IsNullOrWhiteSpace(ruleId))
            {
                throw new ArgumentException(
                    "Rule ID must not be null, empty, or whitespace.",
                    nameof(ruleId));
            }

            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Target ID must not be null, empty, or whitespace.",
                    nameof(targetId));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

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
            TargetId = targetId;
            Priority = priority;
            Condition = condition;
            Effects = effects;
        }
    }
}