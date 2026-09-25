using System;
using ShopGame.Core.EventBus;

namespace ShopGame.Interaction.Runtime
{
    public sealed class KnowledgeSlot
    {
        private readonly EventBus eventBus;

        private string selectedKnowledgeId;
        private string selectedKnowledgeDisplayName;

        public bool HasSelection =>
            selectedKnowledgeId != null;

        public string SelectedKnowledgeId =>
            selectedKnowledgeId;

        public string SelectedKnowledgeDisplayName =>
            selectedKnowledgeDisplayName;

        public KnowledgeSlot(EventBus eventBus)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public void SelectOrToggle(
    string knowledgeId,
    string displayName)
        {
            ValidateKnowledgeId(knowledgeId);

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    "Knowledge display name must not be null, empty, or whitespace.",
                    nameof(displayName));
            }

            if (selectedKnowledgeId == knowledgeId)
            {
                selectedKnowledgeId = null;
                selectedKnowledgeDisplayName = null;
            }
            else
            {
                selectedKnowledgeId = knowledgeId;
                selectedKnowledgeDisplayName = displayName;
            }

            eventBus.Publish(
                new KnowledgeSlotChangedEvent());
        }

        public void Clear()
        {
            if (selectedKnowledgeId == null)
                return;

            selectedKnowledgeId = null;
            selectedKnowledgeDisplayName = null;

            eventBus.Publish(
                new KnowledgeSlotChangedEvent());
        }

        private static void ValidateKnowledgeId(
            string knowledgeId)
        {
            if (string.IsNullOrWhiteSpace(knowledgeId))
            {
                throw new ArgumentException(
                    "Knowledge ID must not be null, empty, or whitespace.",
                    nameof(knowledgeId));
            }
        }
    }
}