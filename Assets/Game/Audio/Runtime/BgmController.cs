using System;
using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Runtime;
using ShopGame.State.Progress;

namespace ShopGame.Sound.Runtime
{
    public sealed class BgmController
    {
        private readonly SoundManager soundManager;
        private readonly GameProgress progress;
        private readonly EventBus eventBus;

        public BgmController(
            SoundManager soundManager,
            GameProgress progress,
            EventBus eventBus)
        {
            this.soundManager =
                soundManager
                ?? throw new ArgumentNullException(nameof(soundManager));

            this.progress =
                progress
                ?? throw new ArgumentNullException(nameof(progress));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            eventBus.Subscribe<DialogueStartedEvent>(
                OnDialogueStarted);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        public void Initialize()
        {
            ApplyDefaultBgm();
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
            switch (eventData.DialogueId)
            {
                case "control_enter":
                case "study_enter":
                    soundManager.PlayBgm(
                        BgmId.SpecialDialogue);
                    break;

                case "blender_core":
                    soundManager.PlayBgm(
                        BgmId.CoreDialogue);
                    break;

                case "loop0":
                    ApplyDefaultBgm();
                    break;
            }
        }

        private void OnDialogueEnded(
            DialogueEndedEvent eventData)
        {
            switch (eventData.DialogueId)
            {
                case "control_enter":
                case "study_enter":
                    ApplyDefaultBgm();
                    break;
            }
        }

        private void ApplyDefaultBgm()
        {
            if (progress.CurrentProgress == 0)
            {
                soundManager.PlayBgm(BgmId.Title);
            }
            else
            {
                soundManager.PlayBgm(BgmId.Default);
            }
        }
    }
}