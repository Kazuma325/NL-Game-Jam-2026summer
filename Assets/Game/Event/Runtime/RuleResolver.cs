using System;
using UnityEngine;
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
    ITrigger trigger,
    GameContext context)
        {
            if (trigger == null)
                throw new ArgumentNullException(nameof(trigger));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Debug.Log(
                $"[RuleResolver] Progress={context.Progress.CurrentProgress}, " +
                $"Slot={(context.KnowledgeSlot.HasSelection ? context.KnowledgeSlot.SelectedKnowledgeId : "<none>")}");

            Rule bestMatch = null;

            foreach (Rule rule in rules)
            {
                if (rule == null)
                    continue;

                if (!IsTriggerMatch(rule.Trigger, trigger))
                {
                    continue;
                }

                bool conditionResult =
                    rule.Condition.Evaluate(context);

                Debug.Log(
                    $"[RuleResolver] Rule={rule.RuleId}, " +
                    $"Priority={rule.Priority}, " +
                    $"Condition={conditionResult}");

                if (!conditionResult)
                    continue;

                if (bestMatch == null ||
                    rule.Priority > bestMatch.Priority)
                {
                    bestMatch = rule;
                }
            }

            Debug.Log(
                $"[RuleResolver] Selected=" +
                $"{(bestMatch != null ? bestMatch.RuleId : "<none>")}");

            return bestMatch;
        }

        private bool IsTriggerMatch(
    ITrigger ruleTrigger,
    ITrigger currentTrigger)
        {
            if (ruleTrigger is InteractionTrigger ruleInteraction &&
                currentTrigger is InteractionTrigger currentInteraction)
            {
                return ruleInteraction.TargetId ==
                       currentInteraction.TargetId;
            }

            if (ruleTrigger is CraftingTrigger ruleCrafting &&
                currentTrigger is CraftingTrigger currentCrafting)
            {
                return ruleCrafting.RecipeId ==
                       currentCrafting.RecipeId;
            }

            return false;
        }
    }
}