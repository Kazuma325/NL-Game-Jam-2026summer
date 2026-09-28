using UnityEngine;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleFactoryDebugTest : MonoBehaviour
    {
        [SerializeField] private TextAsset ruleText;

        private void Start()
        {
            if (ruleText == null)
            {
                Debug.LogError(
                    "RuleFactoryDebugTest: Rule text is not assigned.");
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

            Debug.Log(
                $"Created runtime rules: {rules.Count}");

            foreach (Rule rule in rules)
            {
                Debug.Log(
                    $"Rule created: " +
                    $"RuleId={rule.RuleId}, " +
                    $"TargetId={rule.Trigger.TargetId}, " +
                    $"Priority={rule.Priority}, " +
                    $"Condition={rule.Condition.GetType().Name}, " +
                    $"Effects={rule.Effects.Count}");

                foreach (IEffect effect in rule.Effects)
                {
                    Debug.Log(
                        $"  Effect={effect.GetType().Name}");
                }
            }
        }
    }
}