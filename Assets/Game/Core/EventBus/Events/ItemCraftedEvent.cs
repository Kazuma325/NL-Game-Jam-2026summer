using System;
using System.Collections.Generic;

namespace ShopGame.Core.EventBus
{
    public readonly struct ItemCraftedEvent
    {
        public IReadOnlyCollection<string> MaterialIds { get; }

        public ItemCraftedEvent(
            IReadOnlyCollection<string> materialIds)
        {
            MaterialIds =
                materialIds
                ?? throw new ArgumentNullException(
                    nameof(materialIds));
        }
    }
}