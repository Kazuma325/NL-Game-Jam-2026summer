using System;
using System.Collections.Generic;
using ShopGame.Event.Data;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleFactory
    {
        private readonly ConditionFactory conditionFactory;
        private readonly EffectFactory effectFactory;

        public RuleFactory(
            ConditionFactory conditionFactory,
            EffectFactory effectFactory)
        {
            this.conditionFactory =
                conditionFactory
                ?? throw new ArgumentNullException(
                    nameof(conditionFactory));

            this.effectFactory =
                effectFactory
                ?? throw new ArgumentNullException(
                    nameof(effectFactory));
        }

        public Rule Create(RuleData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            ICondition condition =
                conditionFactory.Create(
                    data.Condition);

            List<IEffect> effects =
                new List<IEffect>();

            foreach (EffectData effectData in data.Effects)
            {
                if (effectData == null)
                {
                    throw new InvalidOperationException(
                        $"Rule '{data.RuleId}' contains a null effect data.");
                }

                effects.Add(
                    effectFactory.Create(
                        effectData));
            }

            ITrigger trigger =
                CreateTrigger(data.Trigger);

            return new Rule(
                data.RuleId,
                trigger,
                condition,
                data.Priority,
                effects);
        }

        private ITrigger CreateTrigger(TriggerData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data.Type switch
            {
                TriggerType.Interaction =>
                    new InteractionTrigger(data.Id),

                TriggerType.Crafting =>
                    new CraftingTrigger(data.Id),

                _ => throw new InvalidOperationException(
                    $"Unsupported trigger type: {data.Type}")
            };
        }

        public IReadOnlyList<Rule> CreateAll(
            IReadOnlyList<RuleData> dataList)
        {
            if (dataList == null)
            {
                throw new ArgumentNullException(
                    nameof(dataList));
            }

            List<Rule> rules =
                new List<Rule>();

            foreach (RuleData data in dataList)
            {
                if (data == null)
                {
                    throw new InvalidOperationException(
                        "Rule data list contains a null element.");
                }

                rules.Add(
                    Create(data));
            }

            return rules;
        }
    }
}