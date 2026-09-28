using System;
using ShopGame.Core.Context;
using ShopGame.Dialogue.Runtime;

namespace ShopGame.Interaction.Runtime
{
    public sealed class OwnerInteractionController
    {
        private const string OwnerTargetId = "owner";

        private readonly DialogueResolver dialogueResolver;
        private readonly DialogueManager dialogueManager;

        public OwnerInteractionController(
            DialogueResolver dialogueResolver,
            DialogueManager dialogueManager)
        {
            this.dialogueResolver =
                dialogueResolver
                ?? throw new ArgumentNullException(
                    nameof(dialogueResolver));

            this.dialogueManager =
                dialogueManager
                ?? throw new ArgumentNullException(
                    nameof(dialogueManager));
        }

        public bool Interact(
            GameContext gameContext)
        {
            if (gameContext == null)
            {
                throw new ArgumentNullException(
                    nameof(gameContext));
            }

            KnowledgeSlot knowledgeSlot =
                gameContext.KnowledgeSlot;

            string knowledgeId =
                knowledgeSlot.HasSelection
                    ? knowledgeSlot.SelectedKnowledgeId
                    : null;

            string knowledgeDisplayName =
                knowledgeSlot.HasSelection
                    ? knowledgeSlot.SelectedKnowledgeDisplayName
                    : null;

            DialogueInteractionContext context =
                new DialogueInteractionContext(
                    OwnerTargetId,
                    knowledgeId,
                    knowledgeDisplayName,
                    gameContext);

            DialogueResolutionResult result =
                dialogueResolver.Resolve(context);

            if (result == null)
            {
                return false;
            }

            return dialogueManager.StartDialogue(result);
        }
    }
}