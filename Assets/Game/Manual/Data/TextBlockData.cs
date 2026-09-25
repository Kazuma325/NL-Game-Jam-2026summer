using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Manual.Data
{
    public sealed class TextBlockData : ManualBlockData
    {
        public IReadOnlyList<ManualInlineData> Inlines { get; }

        public TextBlockData(IReadOnlyList<ManualInlineData> inlines)
        {
            Inlines = inlines ?? throw new ArgumentNullException(nameof(inlines));
        }
    }
}

