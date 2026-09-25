using System;
using UnityEngine;

namespace ShopGame.Interaction.Runtime
{
    public sealed class KnowledgeSelectionController
    {
        private readonly KnowledgeSlot knowledgeSlot;

        public KnowledgeSelectionController(KnowledgeSlot knowledgeSlot)
        {
            this.knowledgeSlot = knowledgeSlot ?? throw new ArgumentNullException(nameof(knowledgeSlot));
        }

        public void SelectOrToggle(
    string knowledgeId,
    string displayName)
        {
            knowledgeSlot.SelectOrToggle(
                knowledgeId,
                displayName);
        }
    }
}