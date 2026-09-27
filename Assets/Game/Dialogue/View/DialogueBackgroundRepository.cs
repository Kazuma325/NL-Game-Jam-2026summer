using System;
using System.Collections.Generic;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueBackgroundRepository
    {
        private readonly Dictionary<string, DialogueBackgroundEntry>
            entriesById;

        public DialogueBackgroundRepository(
            IReadOnlyList<DialogueBackgroundEntry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            entriesById =
                new Dictionary<string, DialogueBackgroundEntry>();

            foreach (DialogueBackgroundEntry entry in entries)
            {
                if (entry == null)
                    continue;

                if (string.IsNullOrWhiteSpace(entry.Id))
                {
                    throw new ArgumentException(
                        "Dialogue background ID must not be null, empty, or whitespace.");
                }

                if (entriesById.ContainsKey(entry.Id))
                {
                    throw new ArgumentException(
                        $"Duplicate dialogue background ID: {entry.Id}");
                }

                if (entry.Sprite == null)
                {
                    throw new ArgumentException(
                        $"Dialogue background '{entry.Id}' " +
                        "has no Sprite assigned.");
                }

                entriesById.Add(
                    entry.Id,
                    entry);
            }
        }

        public bool Contains(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            return entriesById.ContainsKey(id);
        }

        public DialogueBackgroundEntry Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Dialogue background ID must not be null, empty, or whitespace.",
                    nameof(id));
            }

            if (!entriesById.TryGetValue(
                    id,
                    out DialogueBackgroundEntry entry))
            {
                throw new KeyNotFoundException(
                    $"Dialogue background ID was not found: {id}");
            }

            return entry;
        }

        public bool TryGet(
            string id,
            out DialogueBackgroundEntry entry)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                entry = null;
                return false;
            }

            return entriesById.TryGetValue(
                id,
                out entry);
        }
    }
}