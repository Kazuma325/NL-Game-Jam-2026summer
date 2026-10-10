using ShopGame.Core.Context;
using ShopGame.Core.EventBus;
using System;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleTriggerController
    {
        private readonly RuleResolver ruleResolver;
        private readonly RuleExecutor ruleExecutor;
        private readonly EventBus eventBus;

        public RuleTriggerController(
    RuleResolver ruleResolver,
    RuleExecutor ruleExecutor,
    EventBus eventBus)
        {
            this.ruleResolver =
                ruleResolver
                ?? throw new ArgumentNullException(
                    nameof(ruleResolver));

            this.ruleExecutor =
                ruleExecutor
                ?? throw new ArgumentNullException(
                    nameof(ruleExecutor));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));
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

            eventBus.Publish(
    new InteractionPerformedEvent());

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