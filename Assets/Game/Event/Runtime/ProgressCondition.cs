using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class ProgressCondition : ICondition
    {
        public enum Operator
        {
            Equals,
            GreaterOrEqual
        }

        private readonly Operator @operator;
        private readonly int value;

        public ProgressCondition(Operator @operator, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Progress value must not be negative.");
            }

            this.@operator = @operator;
            this.value = value;
        }

        public bool Evaluate(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            int currentProgress = context.Progress.CurrentProgress;

            return @operator switch
            {
                Operator.Equals => currentProgress == value,
                Operator.GreaterOrEqual => currentProgress >= value,

                _ => throw new InvalidOperationException(
                    $"Unsupported progress operator: {@operator}")
            };
        }
    }
}