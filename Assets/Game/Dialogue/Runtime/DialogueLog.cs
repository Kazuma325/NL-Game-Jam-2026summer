using System;
using System.Collections.Generic;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueLog
    {
        private readonly List<DialogueLogEntry> entries = new();

        public IReadOnlyList<DialogueLogEntry> Entries =>
            entries;

        public void Add(
            string speakerName,
            string text)
        {
            entries.Add(
                new DialogueLogEntry(
                    speakerName,
                    text));
        }

        public void UpdateCurrentEntry(
            string speakerName,
            string text)
        {
            if (entries.Count == 0)
            {
                Add(
                    speakerName,
                    text);

                return;
            }

            entries[entries.Count - 1] =
                new DialogueLogEntry(
                    speakerName,
                    text);
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}