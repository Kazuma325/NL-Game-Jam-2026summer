using System;
using System.Collections.Generic;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueRepository
    {
        private readonly Dictionary<string, IReadOnlyList<DialogueLine>>
            linesByDialogueId;

        public DialogueRepository(
            IReadOnlyList<DialogueLine> dialogueLines)
        {
            if (dialogueLines == null)
            {
                throw new ArgumentNullException(
                    nameof(dialogueLines));
            }

            linesByDialogueId =
                new Dictionary<string, IReadOnlyList<DialogueLine>>();

            BuildRepository(dialogueLines);
        }

        public bool Contains(string dialogueId)
        {
            ValidateDialogueId(dialogueId);

            return linesByDialogueId.ContainsKey(dialogueId);
        }

        public IReadOnlyList<DialogueLine> GetLines(
            string dialogueId)
        {
            ValidateDialogueId(dialogueId);

            if (!linesByDialogueId.TryGetValue(
                    dialogueId,
                    out IReadOnlyList<DialogueLine> lines))
            {
                throw new KeyNotFoundException(
                    $"Dialogue '{dialogueId}' was not found.");
            }

            return lines;
        }

        private void BuildRepository(
            IReadOnlyList<DialogueLine> dialogueLines)
        {
            var groupedLines =
                new Dictionary<string, List<DialogueLine>>();

            foreach (DialogueLine line in dialogueLines)
            {
                if (line == null)
                {
                    throw new ArgumentException(
                        "Dialogue line collection contains a null entry.",
                        nameof(dialogueLines));
                }

                if (!groupedLines.TryGetValue(
                        line.DialogueId,
                        out List<DialogueLine> lines))
                {
                    lines = new List<DialogueLine>();

                    groupedLines.Add(
                        line.DialogueId,
                        lines);
                }

                lines.Add(line);
            }

            foreach (KeyValuePair<string, List<DialogueLine>> pair
                     in groupedLines)
            {
                pair.Value.Sort(
                    (left, right) =>
                        left.Index.CompareTo(right.Index));

                linesByDialogueId.Add(
                    pair.Key,
                    pair.Value.AsReadOnly());
            }
        }

        private static void ValidateDialogueId(
            string dialogueId)
        {
            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new ArgumentException(
                    "Dialogue ID must not be null, empty, or whitespace.",
                    nameof(dialogueId));
            }
        }
    }
}