using System;
using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Runtime;
using UnityEngine;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueLogViewPresenter : MonoBehaviour
    {
        [SerializeField] private DialogueLogView logView;

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

            if (logView == null)
            {
                throw new InvalidOperationException(
                    "Dialogue Log View is not assigned.");
            }

            eventBus.Subscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);

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
            logView.Clear();
        }

        public void Refresh()
        {
            if (dialogueManager == null)
            {
                logView.Clear();
                return;
            }

            dialogueManager.UpdateCurrentLogEntry();

            logView.Display(
                dialogueManager.LogEntries);
        }

        public void Open()
        {
            if (dialogueManager == null)
                return;

            dialogueManager.Pause();

            logView.Display(
                dialogueManager.LogEntries);

            logView.Open();
        }

        public void Close()
        {
            logView.Close();

            dialogueManager.Resume();
        }
    }
}