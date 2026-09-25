using System;
using ShopGame.Interaction.Runtime;
using UnityEngine;

namespace ShopGame.Adventure.Runtime
{
    public sealed class Door : IKnowledgeUsable
    {
        private readonly string connectionId;
        private readonly string requiredKnowledgeId;

        public string ConnectionId => connectionId;

        public Door(string connectionId, string requireKnowledgeId)
        {
            if(string.IsNullOrEmpty(connectionId)) throw new ArgumentException("Connection ID must not be null, empty, or whitespace.", nameof(connectionId));
            if (string.IsNullOrEmpty(requireKnowledgeId)) throw new ArgumentException("Required knowledge ID must not be null, empty, or whitespace.", nameof(requiredKnowledgeId));

            this.connectionId = connectionId;
            this.requiredKnowledgeId = requireKnowledgeId;
        }

        public KnowledgeUseResult TryUseKnowledge(string knowledgeId)
        {
            if(string.IsNullOrEmpty(knowledgeId)) return KnowledgeUseResult.Failed;

            if(knowledgeId != requiredKnowledgeId) return KnowledgeUseResult.Failed;

            return KnowledgeUseResult.Success;
        }
    }
}
