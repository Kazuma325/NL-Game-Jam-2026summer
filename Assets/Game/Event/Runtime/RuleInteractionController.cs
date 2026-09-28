using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleInteractionController
    {
        private readonly RuleResolver ruleResolver;
        private readonly RuleExecutor ruleExecutor;

        public RuleInteractionController(
            RuleResolver ruleResolver,
            RuleExecutor ruleExecutor)
        {
            this.ruleResolver =
                ruleResolver
                ?? throw new ArgumentNullException(
                    nameof(ruleResolver));

            this.ruleExecutor =
                ruleExecutor
                ?? throw new ArgumentNullException(
                    nameof(ruleExecutor));
        }

        public bool TryInteract(
            string targetId,
            GameContext context)
        {
            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Target ID must not be null, empty, or whitespace.",
                    nameof(targetId));
            }

            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            InteractionTrigger trigger =
                new InteractionTrigger(
                    targetId);

            Rule rule =
                ruleResolver.Resolve(
                    trigger,
                    context);

            if (rule == null)
            {
                return false;
            }

            return ruleExecutor.Execute(
                rule,
                context);
        }
    }
}