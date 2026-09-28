using System;
using ShopGame.Event.Data;

namespace ShopGame.Event.Runtime
{
    public sealed class EffectFactory
    {
        public IEffect Create(EffectData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            switch (data.Type)
            {
                case EffectType.StartDialogue:
                    return CreateStartDialogue(data);

                case EffectType.AdvanceProgress:
                    return CreateAdvanceProgress(data);

                case EffectType.RequestLoopEndAfterDialogue:
                    return CreateRequestLoopEndAfterDialogue(data);

                default:
                    throw new InvalidOperationException(
                        $"Unsupported effect type: {data.Type}");
            }
        }

        private IEffect CreateStartDialogue(
            EffectData data)
        {
            return new StartDialogueEffect(
                data.StringValue);
        }

        private IEffect CreateAdvanceProgress(
            EffectData data)
        {
            return new AdvanceProgressEffect(
                data.IntValue);
        }

        private IEffect CreateRequestLoopEndAfterDialogue(
            EffectData data)
        {
            return new RequestLoopEndAfterDialogueEffect();
        }
    }
}