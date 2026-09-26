using System;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueLine
    {
        public string DialogueId { get; }
        public int Index { get; }
        public string SpeakerName { get; }
        public string Text { get; }
        public string PortraitId { get; }
        public string BackgroundId { get; }
        public bool StopSkip { get; }

        public DialogueLine(
            string dialogueId,
            int index,
            string speakerName,
            string text,
            string portraitId,
            string backgroundId,
            bool stopSkip)
        {
            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new ArgumentException(
                    "Dialogue ID must not be null, empty, or whitespace.",
                    nameof(dialogueId));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    "Dialogue index must not be negative.");
            }

            DialogueId = dialogueId;
            Index = index;
            SpeakerName = speakerName ?? string.Empty;
            Text = text ?? string.Empty;
            PortraitId = portraitId ?? string.Empty;
            BackgroundId = backgroundId ?? string.Empty;
            StopSkip = stopSkip;
        }
    }
}