using System;
using System.Collections.Generic;

namespace ShopGame.Manual.Data
{
    public sealed class NoteBlockData : ManualBlockData
    {
        public IReadOnlyList<ManualInlineData> Inlines { get; }

        public NoteBlockData(
            IReadOnlyList<ManualInlineData> inlines)
        {
            Inlines = inlines ?? throw new ArgumentNullException(nameof(inlines));
        }
    }
}