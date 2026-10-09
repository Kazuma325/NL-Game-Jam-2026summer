using ShopGame.Core.Context;
using System;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleTriggerController
    {
        private readonly RuleResolver ruleResolver;
        private readonly RuleExecutor ruleExecutor;

        public RuleTriggerController(
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

        public bool TryTrigger(
            ITrigger trigger,
            GameContext context)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(
                    nameof(trigger));
            }

            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            Rule rule =
                ruleResolver.Resolve(
                    trigger,
                    context);

            if (rule == null)
            {
                return false;
            }

            ruleExecutor.Execute(
                rule,
                context);

            return true;
        }
    }
}