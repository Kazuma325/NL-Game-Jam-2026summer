using System;
using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Runtime;
using UnityEngine;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueViewPresenter : MonoBehaviour
    {
        [SerializeField] private DialogueView dialogueView;

        private DialogueManager dialogueManager;
        private EventBus eventBus;

        public void Initialize(
            DialogueManager dialogueManager,
            EventBus eventBus)
        {
            this.dialogueManager =
                dialogueManager
                ?? throw new ArgumentNullException(
                    nameof(dialogueManager));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            if (dialogueView == null)
            {
                throw new InvalidOperationException(
                    "Dialogue View is not assigned.");
            }

            eventBus.Subscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);

            Refresh();
        }

        private void Update()
        {
            if (dialogueManager == null)
                return;

            dialogueManager.Update(Time.deltaTime);

            Refresh();
        }

        private void OnDestroy()
        {
            if (eventBus == null)
                return;

            eventBus.Unsubscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Unsubscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        private void OnDialogueStarted(
            DialogueStartedEvent eventData)
        {
            Refresh();
        }

        private void OnDialogueEnded(
            DialogueEndedEvent eventData)
        {
            dialogueView.Clear();
        }

        private void Refresh()
        {
            if (dialogueManager == null)
            {
                dialogueView.Clear();
                return;
            }

            if (!dialogueManager.IsPlaying)
            {
                dialogueView.Clear();
                return;
            }

            DialogueLine currentLine =
                dialogueManager.CurrentLine;

            if (currentLine == null)
            {
                dialogueView.Clear();
                return;
            }

            dialogueView.DisplayLine(
                currentLine,
                dialogueManager.CurrentCharacterIndex);
        }
    }
}