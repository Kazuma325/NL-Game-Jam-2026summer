using System;
using ShopGame.Core.Context;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueInteractionContext
    {
        public string TargetId { get; }

        public string KnowledgeId { get; }

        public string KnowledgeDisplayName { get; }

        public GameContext GameContext { get; }

        public DialogueInteractionContext(
            string targetId,
            string knowledgeId,
            string knowledgeDisplayName,
            GameContext gameContext)
        {
            if (string.IsNullOrWhiteSpace(targetId))
            {
                throw new ArgumentException(
                    "Target ID must not be null, empty, or whitespace.",
                    nameof(targetId));
            }

            GameContext =
                gameContext
                ?? throw new ArgumentNullException(nameof(gameContext));

            TargetId = targetId;
            KnowledgeId = knowledgeId;
            KnowledgeDisplayName = knowledgeDisplayName;
        }
    }
}