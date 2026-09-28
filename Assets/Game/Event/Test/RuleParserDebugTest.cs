using UnityEngine;

namespace ShopGame.Event.Runtime
{
    public sealed class RuleParserDebugTest : MonoBehaviour
    {
        [SerializeField] private TextAsset ruleText;

        private void Start()
        {
            if (ruleText == null)
            {
                Debug.LogError(
                    "RuleParserDebugTest: Rule text is not assigned.");
                return;
            }

            RuleParser parser = new RuleParser();

            var rules = parser.Parse(
                ruleText.text);

            Debug.Log(
                $"Parsed rules: {rules.Count}");

            foreach (var rule in rules)
            {
                Debug.Log(
                    $"RuleId={rule.RuleId}, " +
                    $"TargetId={rule.TargetId}, " +
                    $"Priority={rule.Priority}, " +
                    $"Conditions={rule.Condition.Type}, " +
                    $"Effects={rule.Effects.Count}");
            }
        }
    }
}