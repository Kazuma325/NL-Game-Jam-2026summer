using System;
using ShopGame.Core.EventBus;
using UnityEngine;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueCanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameObject dialogueCanvas;

        private EventBus eventBus;

        public void Initialize(EventBus eventBus)
        {
            if (eventBus == null)
            {
                throw new ArgumentNullException(
                    nameof(eventBus));
            }

            if (dialogueCanvas == null)
            {
                throw new InvalidOperationException(
                    "Dialogue Canvas is not assigned.");
            }

            this.eventBus = eventBus;

            eventBus.Subscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);

            dialogueCanvas.SetActive(false);
        }

        private void OnDialogueStarted(DialogueStartedEvent eventData)
        {
            UnityEngine.Debug.Log(
                $"DialogueCanvasController: DialogueStarted '{eventData.DialogueId}'");

            dialogueCanvas.SetActive(true);
        }

        private void OnDialogueEnded(DialogueEndedEvent eventData)
        {
            UnityEngine.Debug.Log(
                $"DialogueCanvasController: DialogueEnded '{eventData.DialogueId}'");

            dialogueCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            if (eventBus == null)
            {
                return;
            }

            eventBus.Unsubscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Unsubscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }
    }
}