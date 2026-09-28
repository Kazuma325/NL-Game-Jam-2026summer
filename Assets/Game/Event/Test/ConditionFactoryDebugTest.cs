using UnityEngine;

namespace ShopGame.Event.Runtime
{
    public sealed class ConditionFactoryDebugTest : MonoBehaviour
    {
        [SerializeField] private TextAsset ruleText;

        private void Start()
        {
            if (ruleText == null)
            {
                Debug.LogError(
                    "ConditionFactoryDebugTest: Rule text is not assigned.");
                return;
            }

            RuleParser parser = new RuleParser();

            var ruleDataList =
                parser.Parse(ruleText.text);

            ConditionFactory factory =
                new ConditionFactory();

            foreach (var ruleData in ruleDataList)
            {
                ICondition condition =
                    factory.Create(ruleData.Condition);

                Debug.Log(
                    $"Condition created: " +
                    $"RuleId={ruleData.RuleId}, " +
                    $"Type={condition.GetType().Name}");
            }
        }
    }
}