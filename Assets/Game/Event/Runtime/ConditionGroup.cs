using System;
using System.Collections.Generic;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class ConditionGroup : ICondition
    {
        public enum Operator
        {
            All,
            Any
        }

        private readonly Operator @operator;
        private readonly IReadOnlyList<ICondition> conditions;

        public ConditionGroup(
            Operator @operator,
            IReadOnlyList<ICondition> conditions)
        {
            if (conditions == null)
                throw new ArgumentNullException(nameof(conditions));

            if (conditions.Count == 0)
                throw new ArgumentException(
                    "Condition group must contain at least one condition.",
                    nameof(conditions));

            this.@operator = @operator;
            this.conditions = conditions;
        }

        public bool Evaluate(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            switch (@operator)
            {
                case Operator.All:
                    foreach (ICondition condition in conditions)
                    {
                        if (!condition.Evaluate(context))
                            return false;
                    }

                    return true;

                case Operator.Any:
                    foreach (ICondition condition in conditions)
                    {
                        if (condition.Evaluate(context))
                            return true;
                    }

                    return false;

                default:
                    throw new InvalidOperationException(
                        $"Unsupported condition operator: {@operator}");
            }
        }
    }
}