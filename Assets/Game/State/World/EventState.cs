using System;
using System.Collections.Generic;

namespace ShopGame.State.World
{
    public sealed class EventState
    {
        private readonly HashSet<string> activeEventIds = new();

        public bool IsActive(string eventId)
        {
            ValidateEventId(eventId);
            return activeEventIds.Contains(eventId);
        }

        public void Activate(string eventId)
        {
            ValidateEventId(eventId);
            activeEventIds.Add(eventId);
        }

        public void Deactivate(string eventId)
        {
            ValidateEventId(eventId);
            activeEventIds.Remove(eventId);
        }

        public void Reset()
        {
            activeEventIds.Clear();
        }

        private static void ValidateEventId(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
            {
                throw new ArgumentException("Event ID must not be null, empty, or whitespace.", nameof(eventId));
            }
        }
    }
}
