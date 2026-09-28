using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class KnowledgeSlotCondition : ICondition
    {
        public enum Operator
        {
            Equals,
            NotEquals
        }

        private readonly Operator @operator;
        private readonly string knowledgeId;

        public KnowledgeSlotCondition(
            Operator @operator,
            string knowledgeId)
        {
            if (string.IsNullOrWhiteSpace(knowledgeId))
            {
                throw new ArgumentException(
                    "Knowledge ID must not be null, empty, or whitespace.",
                    nameof(knowledgeId));
            }

            this.@operator = @operator;
            this.knowledgeId = knowledgeId;
        }

        public bool Evaluate(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            bool isSelected =
                context.KnowledgeSlot.HasSelection &&
                context.KnowledgeSlot.SelectedKnowledgeId == knowledgeId;

            return @operator switch
            {
                Operator.Equals => isSelected,
                Operator.NotEquals => !isSelected,

                _ => throw new InvalidOperationException(
                    $"Unsupported knowledge slot operator: {@operator}")
            };
        }
    }
}