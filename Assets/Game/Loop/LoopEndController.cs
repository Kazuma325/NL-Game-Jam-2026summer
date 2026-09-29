using ShopGame.Core.EventBus;
using ShopGame.State.Progress;
using System;

namespace ShopGame.Loop
{
    public sealed class LoopEndController
    {
        private readonly LoopManager loopManager;
        private readonly LoopTimer loopTimer;
        private readonly GameProgress progress;
        private readonly EventBus eventBus;

        private bool isDialogueActive;

        public LoopEndReason PendingReason { get; private set; }

        public bool HasPendingEnd =>
            PendingReason != LoopEndReason.None;

        public LoopEndController(
            LoopManager loopManager,
            LoopTimer loopTimer,
            GameProgress progress,
            EventBus eventBus)
        {
            this.loopManager =
                loopManager
                ?? throw new ArgumentNullException(
                    nameof(loopManager));

            this.loopTimer =
                loopTimer
                ?? throw new ArgumentNullException(
                    nameof(loopTimer));

            this.progress =
    progress
    ?? throw new ArgumentNullException(nameof(progress));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            PendingReason = LoopEndReason.None;

            eventBus.Subscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        public void Update()
        {
            if (!loopManager.IsLoopActive)
                return;

            if (progress.CurrentProgress == 0)
                return;

            if (!loopTimer.IsTimeLimitReached)
                return;

            RequestEnd(LoopEndReason.TimeLimit);
        }

        public void RequestEnd(
            LoopEndReason reason)
        {
            if (reason == LoopEndReason.None)
            {
                throw new ArgumentException(
                    "Loop end reason must not be None.",
                    nameof(reason));
            }

            if (!loopManager.IsLoopActive)
            {
                return;
            }

            if (HasPendingEnd)
            {
                return;
            }

            PendingReason = reason;

            TryEndLoop();
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Unsubscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        private void OnDialogueStarted(
            DialogueStartedEvent eventData)
        {
            isDialogueActive = true;
        }

        private void OnDialogueEnded(
            DialogueEndedEvent eventData)
        {
            isDialogueActive = false;

            if (eventData.EndLoopOnComplete)
            {
                RequestEnd(LoopEndReason.DialogueCompleted);
                return;
            }

            TryEndLoop();
        }

        private void TryEndLoop()
        {
            if (!HasPendingEnd)
            {
                return;
            }

            if (isDialogueActive)
            {
                return;
            }

            loopManager.EndLoop();

            PendingReason = LoopEndReason.None;
        }
    }
}