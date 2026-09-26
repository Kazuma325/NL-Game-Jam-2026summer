using System;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueLogEntry
    {
        public string SpeakerName { get; }
        public string Text { get; }

        public DialogueLogEntry(
            string speakerName,
            string text)
        {
            SpeakerName =
                speakerName ?? string.Empty;

            Text =
                text ?? string.Empty;
        }
    }
}