using System;
using System.Collections.Generic;
using ShopGame.Core.Context;
using ShopGame.Dialogue.Data;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueResolver
    {
        private readonly IReadOnlyList<DialogueData> dialogueDataList;

        public DialogueResolver(
            IReadOnlyList<DialogueData> dialogueDataList)
        {
            this.dialogueDataList =
                dialogueDataList
                ?? throw new ArgumentNullException(nameof(dialogueDataList));
        }

        public DialogueResolutionResult Resolve(
            DialogueInteractionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            DialogueData bestMatch = null;

            foreach (DialogueData dialogueData in dialogueDataList)
            {
                if (dialogueData == null)
                    continue;

                if (!MatchesTarget(dialogueData, context))
                    continue;

                if (!MatchesKnowledge(dialogueData, context))
                    continue;

                if (!MatchesProgress(dialogueData, context))
                    continue;

                if (!MatchesLoop(dialogueData, context))
                    continue;

                if (!MatchesWorld(dialogueData, context))
                    continue;

                if (!MatchesCrafting(dialogueData, context))
                    continue;

                if (bestMatch == null ||
                    dialogueData.Priority > bestMatch.Priority)
                {
                    bestMatch = dialogueData;
                }
            }

            if (bestMatch == null)
                return null;

            return new DialogueResolutionResult(
                bestMatch.DialogueId,
                bestMatch.ConsumeKnowledge,
                bestMatch.EndLoopOnComplete);
        }

        private static bool MatchesTarget(
            DialogueData dialogueData,
            DialogueInteractionContext context)
        {
            return dialogueData.TargetId == context.TargetId;
        }

        private static bool MatchesKnowledge(
            DialogueData dialogueData,
            DialogueInteractionContext context)
        {
            DialogueKnowledgeCondition condition =
                dialogueData.KnowledgeCondition;

            if (condition == null)
                return false;

            switch (condition.Type)
            {
                case DialogueKnowledgeConditionType.Any:
                    return true;

                case DialogueKnowledgeConditionType.Empty:
                    return context.KnowledgeId == null;

                case DialogueKnowledgeConditionType.Specific:
                    return context.KnowledgeId ==
                           condition.KnowledgeId;

                case DialogueKnowledgeConditionType.NotSpecific:
                    return context.KnowledgeId !=
                        condition.KnowledgeId;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool MatchesProgress(
            DialogueData dialogueData,
            DialogueInteractionContext context)
        {
            return true;
        }

        private static bool MatchesLoop(
            DialogueData dialogueData,
            DialogueInteractionContext context)
        {
            int loopCount = context.GameContext.Loop.LoopCount;

            if (loopCount < dialogueData.MinimumLoopCount)
                return false;

            if (dialogueData.MaximumLoopCount >= 0 &&
                loopCount > dialogueData.MaximumLoopCount)
            {
                return false;
            }

            return true;
        }

        private static bool MatchesWorld(
            DialogueData dialogueData,
            DialogueInteractionContext context)
        {
            var world = context.GameContext.World;

            if (!string.IsNullOrWhiteSpace(
                    dialogueData.RequiredRoomId))
            {
                if (world.CurrentRoomId !=
                    dialogueData.RequiredRoomId)
                {
                    return false;
                }
            }

            if (!string.IsNullOrWhiteSpace(
                    dialogueData.RequiredDeviceStatus))
            {
                string currentStatus =
                    world.Bomb.Status.ToString();

                if (currentStatus !=
                    dialogueData.RequiredDeviceStatus)
                {
                    return false;
                }
            }

            foreach (string eventId
                     in dialogueData.RequiredActiveEventIds)
            {
                if (!world.Events.IsActive(eventId))
                    return false;
            }

            return true;
        }

        private static bool MatchesCrafting(
    DialogueData dialogueData,
    DialogueInteractionContext context)
        {
            DialogueCraftingCondition condition =
                dialogueData.CraftingCondition;

            if (condition == null)
                return false;

            switch (condition.Type)
            {
                case DialogueCraftingConditionType.None:
                    return true;

                case DialogueCraftingConditionType.SpecificItem:
                    return context.IsCraftingContext &&
                           context.CraftedItemId ==
                           condition.ItemId;

                case DialogueCraftingConditionType.NoRecognizedRecipe:
                    return context.IsCraftingContext &&
                           context.CraftedItemId == null;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}