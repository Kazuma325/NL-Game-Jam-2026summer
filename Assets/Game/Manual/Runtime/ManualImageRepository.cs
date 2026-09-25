using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualImageRepository
    {
        private readonly Dictionary<string, Sprite> spritesByPath;

        public ManualImageRepository(
            IReadOnlyCollection<ManualImageEntry> entries)
        {
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            spritesByPath =
                new Dictionary<string, Sprite>();

            foreach (ManualImageEntry entry in entries)
            {
                if (entry == null)
                {
                    throw new ArgumentException(
                        "Manual image collection must not contain null entries.",
                        nameof(entries));
                }

                if (string.IsNullOrWhiteSpace(entry.Path))
                {
                    throw new ArgumentException(
                        "Manual image path must not be empty.",
                        nameof(entries));
                }

                if (entry.Sprite == null)
                {
                    throw new ArgumentException(
                        $"Sprite is not assigned: {entry.Path}",
                        nameof(entries));
                }

                if (spritesByPath.ContainsKey(entry.Path))
                {
                    throw new ArgumentException(
                        $"Duplicate manual image path: {entry.Path}",
                        nameof(entries));
                }

                spritesByPath.Add(
                    entry.Path,
                    entry.Sprite);
            }
        }

        public Sprite GetImage(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(
                    "Image path must not be null, empty, or whitespace.",
                    nameof(path));

            if (!spritesByPath.TryGetValue(
                path,
                out Sprite sprite))
            {
                throw new InvalidOperationException(
                    $"Manual image was not found: {path}");
            }

            return sprite;
        }
    }

    [Serializable]
    public sealed class ManualImageEntry
    {
        [SerializeField]
        private string path;

        [SerializeField]
        private Sprite sprite;

        public string Path => path;
        public Sprite Sprite => sprite;
    }
}