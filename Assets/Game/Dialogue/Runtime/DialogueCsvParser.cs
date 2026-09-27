using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueCsvParser
    {
        private static readonly string[] ExpectedHeaders =
        {
            "DialogueId",
            "Index",
            "SpeakerName",
            "Text",
            "PortraitId",
            "BackgroundId",
            "StopSkip"
        };

        public IReadOnlyList<DialogueLine> Parse(string csvText)
        {
            if (string.IsNullOrWhiteSpace(csvText))
            {
                throw new ArgumentException(
                    "CSV text must not be null, empty, or whitespace.",
                    nameof(csvText));
            }

            List<List<string>> records = ParseRecords(csvText);

            if (records.Count == 0)
            {
                throw new FormatException("CSV does not contain any records.");
            }

            ValidateHeader(records[0]);

            var lines = new List<DialogueLine>();

            for (int i = 1; i < records.Count; i++)
            {
                List<string> record = records[i];

                if (IsEmptyRecord(record))
                {
                    continue;
                }

                lines.Add(ParseLine(record, i + 1));
            }

            ValidateIndices(lines);

            return lines;
        }

        private static DialogueLine ParseLine(
            IReadOnlyList<string> record,
            int csvLineNumber)
        {
            if (record.Count != ExpectedHeaders.Length)
            {
                throw new FormatException(
                    $"CSV line {csvLineNumber} must contain " +
                    $"{ExpectedHeaders.Length} columns, " +
                    $"but contained {record.Count}.");
            }

            string dialogueId = record[0];

            if (!int.TryParse(
                    record[1],
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out int index))
            {
                throw new FormatException(
                    $"CSV line {csvLineNumber}: " +
                    $"Index '{record[1]}' is not a valid integer.");
            }

            if (!bool.TryParse(record[6], out bool stopSkip))
            {
                throw new FormatException(
                    $"CSV line {csvLineNumber}: " +
                    $"StopSkip '{record[6]}' must be True or False.");
            }

            try
            {
                return new DialogueLine(
                    dialogueId,
                    index,
                    record[2],
                    record[3].Replace("\\n", "\n"),
                    record[4],
                    record[5],
                    stopSkip);
            }
            catch (Exception exception)
            {
                throw new FormatException(
                    $"CSV line {csvLineNumber} contains invalid dialogue data.",
                    exception);
            }
        }

        private static void ValidateHeader(
            IReadOnlyList<string> header)
        {
            if (header.Count != ExpectedHeaders.Length)
            {
                throw new FormatException(
                    "CSV header contains an unexpected number of columns.");
            }

            for (int i = 0; i < ExpectedHeaders.Length; i++)
            {
                if (header[i] != ExpectedHeaders[i])
                {
                    throw new FormatException(
                        $"CSV header column {i + 1} must be " +
                        $"'{ExpectedHeaders[i]}', " +
                        $"but was '{header[i]}'.");
                }
            }
        }

        private static void ValidateIndices(
            IReadOnlyList<DialogueLine> lines)
        {
            var linesByDialogueId =
                new Dictionary<string, List<DialogueLine>>();

            foreach (DialogueLine line in lines)
            {
                if (!linesByDialogueId.TryGetValue(
                        line.DialogueId,
                        out List<DialogueLine> dialogueLines))
                {
                    dialogueLines = new List<DialogueLine>();
                    linesByDialogueId.Add(
                        line.DialogueId,
                        dialogueLines);
                }

                dialogueLines.Add(line);
            }

            foreach (KeyValuePair<string, List<DialogueLine>> pair
                     in linesByDialogueId)
            {
                List<DialogueLine> dialogueLines = pair.Value;

                dialogueLines.Sort(
                    (left, right) => left.Index.CompareTo(right.Index));

                for (int i = 0; i < dialogueLines.Count; i++)
                {
                    DialogueLine line = dialogueLines[i];

                    if (line.Index != i)
                    {
                        throw new FormatException(
                            $"Dialogue '{pair.Key}' must use consecutive " +
                            $"indices starting at 0. " +
                            $"Expected index {i}, but found {line.Index}.");
                    }
                }
            }
        }

        private static bool IsEmptyRecord(
            IReadOnlyList<string> record)
        {
            foreach (string value in record)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<List<string>> ParseRecords(
            string csvText)
        {
            var records = new List<List<string>>();
            var currentRecord = new List<string>();
            var currentField = new System.Text.StringBuilder();

            bool insideQuotes = false;

            for (int i = 0; i < csvText.Length; i++)
            {
                char character = csvText[i];

                if (character == '"')
                {
                    if (insideQuotes &&
                        i + 1 < csvText.Length &&
                        csvText[i + 1] == '"')
                    {
                        currentField.Append('"');
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }

                    continue;
                }

                if (character == ',' && !insideQuotes)
                {
                    currentRecord.Add(currentField.ToString());
                    currentField.Clear();
                    continue;
                }

                if ((character == '\n' || character == '\r') &&
                    !insideQuotes)
                {
                    currentRecord.Add(currentField.ToString());
                    currentField.Clear();

                    if (character == '\r' &&
                        i + 1 < csvText.Length &&
                        csvText[i + 1] == '\n')
                    {
                        i++;
                    }

                    records.Add(currentRecord);
                    currentRecord = new List<string>();
                    continue;
                }

                currentField.Append(character);
            }

            if (insideQuotes)
            {
                throw new FormatException(
                    "CSV contains an unclosed quoted field.");
            }

            if (currentField.Length > 0 ||
                currentRecord.Count > 0)
            {
                currentRecord.Add(currentField.ToString());
                records.Add(currentRecord);
            }

            return records;
        }
    }
}