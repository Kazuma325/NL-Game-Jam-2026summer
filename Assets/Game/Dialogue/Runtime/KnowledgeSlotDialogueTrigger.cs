using System;
using System.Collections.Generic;
using ShopGame.Core.Context;
using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Data;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class KnowledgeSlotDialogueTrigger
    {
        private readonly GameContext gameContext;
        private readonly IReadOnlyList<KnowledgeSlotDialogueTriggerData> triggerDataList;

        public KnowledgeSlotDialogueTrigger(
            GameContext gameContext,
            IReadOnlyList<KnowledgeSlotDialogueTriggerData> triggerDataList)
        {
            this.gameContext =
                gameContext
                ?? throw new ArgumentNullException(nameof(gameContext));

            this.triggerDataList =
                triggerDataList
                ?? throw new ArgumentNullException(nameof(triggerDataList));
        }

        public void Initialize()
        {
            gameContext.EventBus.Subscribe<KnowledgeSlotChangedEvent>(
                OnKnowledgeSlotChanged);
        }

        public void Dispose()
        {
            gameContext.EventBus.Unsubscribe<KnowledgeSlotChangedEvent>(
                OnKnowledgeSlotChanged);
        }

        private void OnKnowledgeSlotChanged(
            KnowledgeSlotChangedEvent eventData)
        {
            string knowledgeId =
                gameContext.KnowledgeSlot.SelectedKnowledgeId;

            KnowledgeSlotDialogueTriggerData triggerData =
                FindTriggerData(knowledgeId);

            if (triggerData == null)
                return;

            var interactionContext =
                new DialogueInteractionContext(
                    triggerData.TargetId,
                    knowledgeId,
                    gameContext.KnowledgeSlot.SelectedKnowledgeDisplayName,
                    gameContext);

            DialogueResolutionResult resolutionResult =
                gameContext.DialogueResolver.Resolve(
                    interactionContext);

            if (resolutionResult == null)
                return;

            gameContext.DialogueManager.StartDialogue(
                resolutionResult);
        }

        private KnowledgeSlotDialogueTriggerData FindTriggerData(
            string knowledgeId)
        {
            if (knowledgeId == null)
                return null;

            foreach (
                KnowledgeSlotDialogueTriggerData triggerData
                in triggerDataList)
            {
                if (triggerData == null)
                    continue;

                if (triggerData.KnowledgeId == knowledgeId)
                    return triggerData;
            }

            return null;
        }
    }
}