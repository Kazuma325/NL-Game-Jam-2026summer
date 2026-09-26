using System;
using System.Collections.Generic;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialoguePlayback
    {
        private readonly DialogueRepository dialogueRepository;

        private IReadOnlyList<DialogueLine> currentLines;

        private string currentDialogueId;
        private int currentLineIndex;
        private int currentCharacterIndex;

        public bool IsPlaying =>
            currentLines != null;

        public string CurrentDialogueId =>
            currentDialogueId;

        public int CurrentLineIndex =>
            currentLineIndex;

        public int CurrentCharacterIndex =>
            currentCharacterIndex;

        public DialogueLine CurrentLine
        {
            get
            {
                if (!IsPlaying)
                    return null;

                return currentLines[currentLineIndex];
            }
        }

        public bool IsLineCompleted
        {
            get
            {
                if (!IsPlaying)
                    return false;

                return currentCharacterIndex >=
                       CurrentLine.Text.Length;
            }
        }

        public bool IsLastLine
        {
            get
            {
                if (!IsPlaying)
                    return false;

                return currentLineIndex ==
                       currentLines.Count - 1;
            }
        }

        public DialoguePlayback(
            DialogueRepository dialogueRepository)
        {
            this.dialogueRepository =
                dialogueRepository
                ?? throw new ArgumentNullException(
                    nameof(dialogueRepository));
        }

        public bool Start(string dialogueId)
        {
            if (IsPlaying)
                return false;

            IReadOnlyList<DialogueLine> lines =
                dialogueRepository.GetLines(dialogueId);

            if (lines.Count == 0)
                return false;

            currentDialogueId = dialogueId;
            currentLines = lines;
            currentLineIndex = 0;
            currentCharacterIndex = 0;

            return true;
        }

        public void CompleteCurrentLine()
        {
            EnsurePlaying();

            currentCharacterIndex =
                CurrentLine.Text.Length;
        }

        public bool MoveToNextLine()
        {
            EnsurePlaying();

            if (!IsLineCompleted)
                return false;

            if (IsLastLine)
                return false;

            currentLineIndex++;
            currentCharacterIndex = 0;

            return true;
        }

        public void AdvanceCharacter()
        {
            EnsurePlaying();

            if (IsLineCompleted)
                return;

            currentCharacterIndex++;
        }

        public void Stop()
        {
            currentDialogueId = null;
            currentLines = null;
            currentLineIndex = 0;
            currentCharacterIndex = 0;
        }

        private void EnsurePlaying()
        {
            if (!IsPlaying)
            {
                throw new InvalidOperationException(
                    "Dialogue playback is not active.");
            }
        }
    }
}