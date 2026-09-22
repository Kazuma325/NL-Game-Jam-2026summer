using System;
using System.Collections.Generic;
using ShopGame.Core.EventBus;

namespace ShopGame.State.Knowledge
{
    public sealed class KnowledgeState
    {
        private readonly EventBus eventBus;
        private readonly HashSet<string> knownKnowledgeIds = new();

        public KnowledgeState(EventBus eventBus)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public bool HasKnowledge(string knowledgeId)
        {
            ValidateKnowledgeId(knowledgeId);
            return knownKnowledgeIds.Contains(knowledgeId);
        }

        public bool AddKnowledge(string knowledgeId)
        {
            ValidateKnowledgeId(knowledgeId);
            if (!knownKnowledgeIds.Add(knowledgeId))
            {
                return false;
            }

            eventBus.Publish(new KnowledgeAddedEvent(knowledgeId));
            return true;
        }

        public IReadOnlyCollection<string> GetAllKnowledge()
        {
            return knownKnowledgeIds;
        }

        private static void ValidateKnowledgeId(string knowledgeId)
        {
            if (string.IsNullOrWhiteSpace(knowledgeId))
            {
                throw new ArgumentException("Knowledge ID must not be null, empty, or whitespace.", nameof(knowledgeId));
            }
        }
    }
}
