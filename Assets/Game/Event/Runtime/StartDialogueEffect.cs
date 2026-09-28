using System;
using ShopGame.Core.Context;

namespace ShopGame.Event.Runtime
{
    public sealed class StartDialogueEffect : IEffect
    {
        private readonly string dialogueId;

        public StartDialogueEffect(string dialogueId)
        {
            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new ArgumentException(
                    "Dialogue ID must not be null, empty, or whitespace.",
                    nameof(dialogueId));
            }

            this.dialogueId = dialogueId;
        }

        public bool Execute(GameContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.DialogueManager.StartDialogue(dialogueId);
        }
    }
}