using UnityEngine;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleInteractionDebugTest : MonoBehaviour
    {
        [SerializeField] private TextAsset ruleText;

        private RuleInteractionController interactionController;

        public void Initialize(
            GameContext context)
        {
            if (context == null)
            {
                Debug.LogError(
                    "RuleInteractionDebugTest: GameContext is null.");
                return;
            }

            if (ruleText == null)
            {
                Debug.LogError(
                    "RuleInteractionDebugTest: Rule text is not assigned.");
                return;
            }

            RuleParser parser =
                new RuleParser();

            var ruleDataList =
                parser.Parse(
                    ruleText.text);

            ConditionFactory conditionFactory =
                new ConditionFactory();

            EffectFactory effectFactory =
                new EffectFactory();

            RuleFactory ruleFactory =
                new RuleFactory(
                    conditionFactory,
                    effectFactory);

            var rules =
                ruleFactory.CreateAll(
                    ruleDataList);

            RuleResolver resolver =
                new RuleResolver(
                    rules);

            RuleExecutor executor =
                new RuleExecutor();

            interactionController =
                new RuleInteractionController(
                    resolver,
                    executor);

            Debug.Log(
                $"Rule interaction system initialized. " +
                $"Rules={rules.Count}");
        }

        public bool TryInteract(
            string targetId,
            GameContext context)
        {
            if (interactionController == null)
            {
                Debug.LogError(
                    "RuleInteractionDebugTest: " +
                    "Interaction system is not initialized.");

                return false;
            }

            return interactionController.TryInteract(
                targetId,
                context);
        }
    }
}