using System;

namespace ShopGame.Event.Data
{
    public sealed class TriggerData
    {
        public TriggerType Type { get; }
        public string Id { get; }

        public TriggerData(
            TriggerType type,
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Trigger ID must not be null, empty, or whitespace.",
                    nameof(id));
            }

            Type = type;
            Id = id;
        }
    }
}