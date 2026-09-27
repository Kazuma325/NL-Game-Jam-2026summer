using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Dialogue.View
{
    public sealed class DialoguePortraitRepository
    {
        private readonly Dictionary<string, DialoguePortraitEntry>
            entriesById;

        public DialoguePortraitRepository(
            IReadOnlyList<DialoguePortraitEntry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            entriesById =
                new Dictionary<string, DialoguePortraitEntry>();

            foreach (DialoguePortraitEntry entry in entries)
            {
                if (entry == null)
                    continue;

                if (string.IsNullOrWhiteSpace(entry.Id))
                {
                    throw new ArgumentException(
                        "Dialogue portrait ID must not be null, empty, or whitespace.");
                }

                if (entriesById.ContainsKey(entry.Id))
                {
                    throw new ArgumentException(
                        $"Duplicate dialogue portrait ID: {entry.Id}");
                }

                ValidateEntry(entry);

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

        public DialoguePortraitEntry Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Dialogue portrait ID must not be null, empty, or whitespace.",
                    nameof(id));
            }

            if (!entriesById.TryGetValue(
                    id,
                    out DialoguePortraitEntry entry))
            {
                throw new KeyNotFoundException(
                    $"Dialogue portrait ID was not found: {id}");
            }

            return entry;
        }

        public bool TryGet(
            string id,
            out DialoguePortraitEntry entry)
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

        private static void ValidateEntry(
            DialoguePortraitEntry entry)
        {
            switch (entry.Type)
            {
                case DialoguePortraitType.Sprite:

                    if (entry.Sprite == null)
                    {
                        throw new ArgumentException(
                            $"Dialogue portrait '{entry.Id}' " +
                            "is configured as Sprite but has no Sprite assigned.");
                    }

                    break;

                case DialoguePortraitType.Prefab:

                    if (entry.Prefab == null)
                    {
                        throw new ArgumentException(
                            $"Dialogue portrait '{entry.Id}' " +
                            "is configured as Prefab but has no Prefab assigned.");
                    }

                    break;

                default:

                    throw new ArgumentOutOfRangeException(
                        nameof(entry),
                        entry.Type,
                        "Unknown dialogue portrait type.");
            }
        }
    }
}