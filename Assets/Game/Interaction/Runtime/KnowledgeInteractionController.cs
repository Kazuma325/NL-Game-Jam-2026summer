using System;

namespace ShopGame.Interaction.Runtime
{
    public sealed class KnowledgeInteractionController
    {
        private readonly KnowledgeSlot knowledgeSlot;

        public KnowledgeInteractionController(KnowledgeSlot knowledgeSlot)
        {
            this.knowledgeSlot = knowledgeSlot ?? throw new ArgumentNullException(nameof(knowledgeSlot));
        }

        public KnowledgeUseResult TryUseKnowledge(IKnowledgeUsable target)
        {
            return TryUseKnowledge(target, null);
        }

        public KnowledgeUseResult TryUseKnowledge(IKnowledgeUsable target, Func<bool> onSuccess)
        {
            if (target == null || !knowledgeSlot.HasSelection)
            {
                return KnowledgeUseResult.NotApplicable;
            }

            KnowledgeUseResult result = target.TryUseKnowledge(knowledgeSlot.SelectedKnowledgeId);
            if(result != KnowledgeUseResult.Success) return result;

            if (onSuccess != null && !onSuccess()) return KnowledgeUseResult.Failed;

            knowledgeSlot.Clear();

            return KnowledgeUseResult.Success;
        }
    }
}
