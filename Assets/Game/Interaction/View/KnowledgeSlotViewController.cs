using System;
using ShopGame.Core.Bootstrap;
using ShopGame.Core.EventBus;
using ShopGame.Interaction.Runtime;
using TMPro;
using UnityEngine;

namespace ShopGame.Interaction.View
{
    public sealed class KnowledgeSlotViewController : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private TMP_Text knowledgeText;

        private void Start()
        {
            if (gameBootstrap == null)
            {
                throw new InvalidOperationException(
                    "GameBootstrap is not assigned.");
            }

            if (knowledgeText == null)
            {
                throw new InvalidOperationException(
                    "Knowledge text is not assigned.");
            }

            gameBootstrap.Context.EventBus.Subscribe<KnowledgeSlotChangedEvent>(HandleKnowledgeSlotChanged);

            Refresh();
        }

        public void Refresh()
        {
            KnowledgeSlot slot =
                gameBootstrap.Context.KnowledgeSlot;

            if (!slot.HasSelection)
            {
                knowledgeText.text = "---";
                return;
            }

            knowledgeText.text =
                slot.SelectedKnowledgeDisplayName;
        }

        private void HandleKnowledgeSlotChanged(
    KnowledgeSlotChangedEvent eventData)
        {
            Refresh();
        }

        private void OnDestroy()
        {
            if (gameBootstrap == null)
                return;

            gameBootstrap.Context.EventBus
                .Unsubscribe<KnowledgeSlotChangedEvent>(
                    HandleKnowledgeSlotChanged);
        }
    }
}