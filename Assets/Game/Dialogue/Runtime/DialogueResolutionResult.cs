using System;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueResolutionResult
    {
        public string DialogueId { get; }

        public bool ConsumeKnowledge { get; }

        public DialogueResolutionResult(
            string dialogueId,
            bool consumeKnowledge)
        {
            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new ArgumentException(
                    "Dialogue ID must not be null, empty, or whitespace.",
                    nameof(dialogueId));
            }

            DialogueId = dialogueId;
            ConsumeKnowledge = consumeKnowledge;
        }
    }
}