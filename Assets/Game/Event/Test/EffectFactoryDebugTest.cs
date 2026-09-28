using UnityEngine;

namespace ShopGame.Event.Runtime
{
    public sealed class EffectFactoryDebugTest : MonoBehaviour
    {
        [SerializeField] private TextAsset ruleText;

        private void Start()
        {
            if (ruleText == null)
            {
                Debug.LogError(
                    "EffectFactoryDebugTest: Rule text is not assigned.");
                return;
            }

            RuleParser parser = new RuleParser();

            var ruleDataList =
                parser.Parse(ruleText.text);

            EffectFactory factory =
                new EffectFactory();

            foreach (var ruleData in ruleDataList)
            {
                foreach (var effectData in ruleData.Effects)
                {
                    IEffect effect =
                        factory.Create(effectData);

                    Debug.Log(
                        $"Effect created: " +
                        $"RuleId={ruleData.RuleId}, " +
                        $"Type={effect.GetType().Name}");
                }
            }
        }
    }
}