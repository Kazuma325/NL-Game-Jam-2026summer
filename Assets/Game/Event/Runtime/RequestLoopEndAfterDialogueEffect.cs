using System;
using ShopGame.Core.Context;
using ShopGame.Loop;

namespace ShopGame.Event.Runtime
{
    public sealed class RequestLoopEndAfterDialogueEffect : IEffect
    {
        public bool Execute(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.LoopEnd.RequestEnd(
                LoopEndReason.DialogueCompleted);

            return true;
        }
    }
}