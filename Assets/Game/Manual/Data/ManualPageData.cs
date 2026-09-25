using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Manual.Data
{
    public sealed class ManualPageData
    {
        public string Id { get; }
        public string Title { get; }
        public IReadOnlyList<string> Keywords { get; }
        public IReadOnlyList<ManualBlockData> Blocks { get; }

        public ManualPageData(string id, string title, IReadOnlyList<string> keywords, IReadOnlyList<ManualBlockData> blocks)
        {
            if(string.IsNullOrEmpty(id)) throw new ArgumentException("Page ID must not be null, empty, or whitespace.", nameof(id));
            if(string.IsNullOrEmpty(title)) throw new ArgumentException("Page title must not be null, empty, or whitespace.", nameof(title));

            Id = id;
            Title = title;
            Keywords = keywords ?? throw new ArgumentNullException(nameof(keywords));
            Blocks = blocks ?? throw new ArgumentNullException(nameof(blocks));
        }
    }
}