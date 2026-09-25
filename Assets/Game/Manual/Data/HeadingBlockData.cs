using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Manual.Data
{
    public sealed class HeadingBlockData : ManualBlockData
    {
        public int Level { get; }
        public IReadOnlyList<ManualInlineData> Inlines { get; }

        public HeadingBlockData(int level, IReadOnlyList<ManualInlineData> inlines)
        {
            if(level < 1 || level > 3) throw new ArgumentOutOfRangeException(nameof(level), level, "Heading level must be between 1 and 3.");

            Inlines = inlines ?? throw new ArgumentNullException(nameof(inlines));
            Level = level;
        }
    }
}