using System;
using System.Collections.Generic;
using ShopGame.Core.EventBus;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueManager
    {
        private readonly EventBus eventBus;
        private readonly DialoguePlayback playback;
        private readonly DialogueTypewriter typewriter;
        private readonly DialogueLog log;

        private bool isPaused;
        private bool currentDialogueEndsLoop;

        public bool IsPlaying =>
            playback.IsPlaying;

        public string CurrentDialogueId =>
            playback.CurrentDialogueId;

        public DialogueLine CurrentLine =>
            playback.CurrentLine;

        public int CurrentLineIndex =>
            playback.CurrentLineIndex;

        public int CurrentCharacterIndex =>
            playback.CurrentCharacterIndex;

        public bool IsLineCompleted =>
            playback.IsLineCompleted;

        public bool IsLastLine =>
            playback.IsLastLine;

        public DialogueSpeed Speed =>
            typewriter.Speed;

        public IReadOnlyList<DialogueLogEntry> LogEntries =>
            log.Entries;

        public bool IsPaused =>
            isPaused;

        public DialogueManager(
            EventBus eventBus,
            DialoguePlayback playback,
            DialogueTypewriter typewriter,
            DialogueLog log)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            this.playback =
                playback
                ?? throw new ArgumentNullException(nameof(playback));

            this.typewriter =
                typewriter
                ?? throw new ArgumentNullException(nameof(typewriter));

            this.log =
                log
                ?? throw new ArgumentNullException(nameof(log));
        }

        public void Update(float deltaTime)
        {
            if (!IsPlaying)
                return;

            if (isPaused)
                return;

            typewriter.Update(
                deltaTime,
                this);
        }

        public bool StartDialogue(
            DialogueResolutionResult resolutionResult)
        {
            if (resolutionResult == null)
            {
                throw new ArgumentNullException(
                    nameof(resolutionResult));
            }

            if (IsPlaying)
                return false;

            bool started =
                playback.Start(
                    resolutionResult.DialogueId);

            if (!started)
                return false;

            currentDialogueEndsLoop =
                resolutionResult.EndLoopOnComplete;

            typewriter.Reset();

            isPaused = false;

            log.Clear();

            DialogueLine currentLine =
                CurrentLine;

            log.Add(
                currentLine.SpeakerName,
                string.Empty);

            eventBus.Publish(
                new DialogueStartedEvent(
                    resolutionResult.DialogueId));

            return true;
        }

        public bool StartDialogue(string dialogueId)
        {
            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new ArgumentException(
                    "Dialogue ID must not be null, empty, or whitespace.",
                    nameof(dialogueId));
            }

            if (IsPlaying)
                return false;

            bool started =
                playback.Start(dialogueId);

            if (!started)
                return false;

            typewriter.Reset();

            isPaused = false;

            log.Clear();

            DialogueLine currentLine =
                CurrentLine;

            log.Add(
                currentLine.SpeakerName,
                string.Empty);

            eventBus.Publish(
                new DialogueStartedEvent(
                    dialogueId));

            return true;
        }

        public void CompleteCurrentLine()
        {
            if (!IsPlaying)
                return;

            playback.CompleteCurrentLine();

            UpdateCurrentLogEntry();
        }

        public bool MoveToNextLine()
        {
            if (!IsPlaying)
                return false;

            UpdateCurrentLogEntry();

            bool moved =
                playback.MoveToNextLine();

            if (!moved)
                return false;

            DialogueLine currentLine =
                CurrentLine;

            log.Add(
                currentLine.SpeakerName,
                string.Empty);

            return true;
        }

        public void AdvanceCharacter()
        {
            if (!IsPlaying)
                return;

            playback.AdvanceCharacter();

            UpdateCurrentLogEntry();
        }

        public void Skip()
        {
            if (!IsPlaying)
                return;

            while (true)
            {
                if (!IsLineCompleted)
                {
                    CompleteCurrentLine();
                }

                if (CurrentLine.StopSkip)
                {
                    return;
                }

                if (IsLastLine)
                {
                    EndDialogue();
                    return;
                }

                bool moved =
                    MoveToNextLine();

                if (!moved)
                    return;
            }
        }

        public void EndDialogue()
        {
            if (!IsPlaying) return;

            string dialogueId = CurrentDialogueId;
            bool endLoopOnComplete = currentDialogueEndsLoop;

            playback.Stop();
            isPaused = false;
            currentDialogueEndsLoop = false;

            eventBus.Publish(
                new DialogueEndedEvent(
                    dialogueId,
                    endLoopOnComplete));
        }

        public void SetSpeed(DialogueSpeed speed)
        {
            typewriter.SetSpeed(speed);
        }

        public void UpdateCurrentLogEntry()
        {
            if (!IsPlaying)
                return;

            DialogueLine currentLine =
                CurrentLine;

            if (currentLine == null)
                return;

            string visibleText =
                currentLine.Text.Substring(
                    0,
                    Math.Min(
                        CurrentCharacterIndex,
                        currentLine.Text.Length));

            log.UpdateCurrentEntry(
                currentLine.SpeakerName,
                visibleText);
        }

        public void Pause()
        {
            if (!IsPlaying)
                return;

            isPaused = true;
        }

        public void Resume()
        {
            if (!IsPlaying)
                return;

            isPaused = false;
        }
    }
}