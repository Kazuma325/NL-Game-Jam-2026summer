using System;
using ShopGame.Core.Context;
using ShopGame.Interaction.Runtime;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueInteractionController
    {
        private readonly KnowledgeSlot knowledgeSlot;
        private readonly DialogueResolver dialogueResolver;

        public DialogueInteractionController(
            KnowledgeSlot knowledgeSlot,
            DialogueResolver dialogueResolver)
        {
            this.knowledgeSlot =
                knowledgeSlot
                ?? throw new ArgumentNullException(nameof(knowledgeSlot));

            this.dialogueResolver =
                dialogueResolver
                ?? throw new ArgumentNullException(nameof(dialogueResolver));
        }

        public DialogueResolutionResult ResolveInteraction(
            string targetId,
            GameContext gameContext)
        {
            if (gameContext == null)
                throw new ArgumentNullException(nameof(gameContext));

            string knowledgeId = null;
            string knowledgeDisplayName = null;

            if (knowledgeSlot.HasSelection)
            {
                knowledgeId =
                    knowledgeSlot.SelectedKnowledgeId;

                knowledgeDisplayName =
                    knowledgeSlot.SelectedKnowledgeDisplayName;
            }

            var context =
                new DialogueInteractionContext(
                    targetId,
                    knowledgeId,
                    knowledgeDisplayName,
                    gameContext);

            return dialogueResolver.Resolve(context);
        }
    }
}