using System;
using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Runtime;
using ShopGame.State.Progress;

namespace ShopGame.Loop
{
    public sealed class LoopDialogueController
    {
        private const string BeginningDialogueId = "beginning";

        private string loopStartDialogueId;

        private readonly GameProgress progress;
        private readonly DialogueManager dialogueManager;
        private readonly LoopTimer loopTimer;
        private readonly EventBus eventBus;

        public LoopDialogueController(
            GameProgress progress,
            DialogueManager dialogueManager,
            LoopTimer loopTimer,
            EventBus eventBus)
        {
            this.progress =
                progress
                ?? throw new ArgumentNullException(nameof(progress));

            this.dialogueManager =
                dialogueManager
                ?? throw new ArgumentNullException(nameof(dialogueManager));

            this.loopTimer =
                loopTimer
                ?? throw new ArgumentNullException(nameof(loopTimer));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            eventBus.Subscribe<LoopStartedEvent>(
                OnLoopStarted);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<LoopStartedEvent>(
                OnLoopStarted);

            eventBus.Unsubscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        public void StartBeginning()
        {
            loopTimer.Stop();

            bool started =
                dialogueManager.StartDialogue(
                    BeginningDialogueId);

            if (!started)
            {
                loopTimer.Start();

                throw new InvalidOperationException(
                    $"Failed to start dialogue '{BeginningDialogueId}'.");
            }
        }

        private void OnLoopStarted(LoopStartedEvent eventData)
        {
            if (eventData.LoopCount == 1)
                return;

            string dialogueId =
                $"loop{progress.CurrentProgress}";

            loopStartDialogueId = dialogueId;

            loopTimer.Stop();

            UnityEngine.Debug.Log(
                $"LoopDialogueController: Starting dialogue '{dialogueId}'");

            bool started =
                dialogueManager.StartDialogue(dialogueId);

            if (!started)
            {
                loopStartDialogueId = null;
                loopTimer.Start();

                throw new InvalidOperationException(
                    $"Failed to start loop dialogue '{dialogueId}'.");
            }
        }

        private void OnDialogueEnded(DialogueEndedEvent eventData)
        {
            if (eventData.DialogueId == BeginningDialogueId)
            {
                loopTimer.Start();
                return;
            }

            if (eventData.DialogueId != loopStartDialogueId)
                return;

            loopStartDialogueId = null;

            if (eventData.EndLoopOnComplete)
                return;

            loopTimer.Start();
        }
    }
}