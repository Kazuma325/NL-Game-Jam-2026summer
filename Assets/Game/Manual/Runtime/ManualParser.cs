using ShopGame.Manual.Data;
using System;
using System.Collections.Generic;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualParser
    {
        private readonly ManualInlineParser inlineParser;

        public ManualParser()
        {
            inlineParser = new ManualInlineParser();
        }

        public ManualPageData Parse(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            string normalizedSource =
                source.Replace("\r\n", "\n")
                      .Replace('\r', '\n');

            string[] lines =
                normalizedSource.Split(
                    '\n',
                    StringSplitOptions.None);

            int currentLine = 0;

            string id = ParseMetadata(
                lines,
                ref currentLine,
                "id");

            string title = ParseMetadata(
                lines,
                ref currentLine,
                "title");

            string keywordsText = ParseMetadata(
                lines,
                ref currentLine,
                "keywords");

            IReadOnlyList<string> keywords = ParseKeywords(keywordsText);

            SkipBlankLines(lines, ref currentLine);

            List<ManualBlockData> blocks =
                ParseBlocks(lines, ref currentLine);

            return new ManualPageData(
                id,
                title,
                keywords,
                blocks);
        }

        private static string ParseMetadata(
            string[] lines,
            ref int currentLine,
            string key)
        {
            while (currentLine < lines.Length &&
                   string.IsNullOrWhiteSpace(lines[currentLine]))
            {
                currentLine++;
            }

            if (currentLine >= lines.Length)
            {
                throw new FormatException(
                    $"Missing metadata: {key}");
            }

            string line = lines[currentLine];

            string prefix = key + ":";

            if (!line.StartsWith(prefix, StringComparison.Ordinal))
            {
                throw new FormatException(
                    $"Expected metadata '{key}:' " +
                    $"at line {currentLine + 1}.");
            }

            string value =
                line.Substring(prefix.Length).Trim();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException(
                    $"Metadata '{key}' must not be empty " +
                    $"at line {currentLine + 1}.");
            }

            currentLine++;

            return value;
        }

        private static IReadOnlyList<string> ParseKeywords(
    string value)
        {
            string[] parts =
                value.Split(',', '、');

            var keywords =
                new List<string>();

            foreach (string part in parts)
            {
                string keyword = part.Trim();

                if (string.IsNullOrWhiteSpace(keyword))
                    continue;

                keywords.Add(keyword);
            }

            if (keywords.Count == 0)
            {
                throw new FormatException(
                    "At least one keyword must be specified.");
            }

            return keywords;
        }

        private static void SkipBlankLines(
            string[] lines,
            ref int currentLine)
        {
            while (currentLine < lines.Length &&
                   string.IsNullOrWhiteSpace(lines[currentLine]))
            {
                currentLine++;
            }
        }

        private List<ManualBlockData> ParseBlocks(
            string[] lines,
            ref int currentLine)
        {
            var blocks = new List<ManualBlockData>();

            while (currentLine < lines.Length)
            {
                if (string.IsNullOrWhiteSpace(lines[currentLine]))
                {
                    currentLine++;
                    continue;
                }

                blocks.Add(
                    ParseBlock(lines, ref currentLine));
            }

            return blocks;
        }

        private ManualBlockData ParseBlock(
            string[] lines,
            ref int currentLine)
        {
            string line = lines[currentLine];

            if (line == "---")
            {
                currentLine++;
                return new SeparatorBlockData();
            }

            if (line.StartsWith("[image:") &&
                line.EndsWith("]"))
            {
                return ParseImageBlock(
                    line,
                    ref currentLine);
            }

            if (line == "[blank]")
            {
                currentLine++;
                return new BlankLineBlockData();
            }

            if (line == "[note]")
            {
                return ParseNoteBlock(
                    lines,
                    ref currentLine);
            }

            if (line.StartsWith("#"))
            {
                return ParseHeadingBlock(
                    line,
                    ref currentLine);
            }

            return ParseTextBlock(
                lines,
                ref currentLine);
        }

        private ManualBlockData ParseHeadingBlock(
            string line,
            ref int currentLine)
        {
            int level = 0;

            while (level < line.Length &&
                   line[level] == '#')
            {
                level++;
            }

            if (level < 1 || level > 3)
            {
                throw new FormatException(
                    $"Unsupported heading level " +
                    $"at line {currentLine + 1}.");
            }

            string text =
                line.Substring(level).Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new FormatException(
                    $"Heading text must not be empty " +
                    $"at line {currentLine + 1}.");
            }

            currentLine++;

            return new HeadingBlockData(
                level,
                inlineParser.Parse(text));
        }

        private static ManualBlockData ParseImageBlock(
            string line,
            ref int currentLine)
        {
            const string prefix = "[image:";
            const string suffix = "]";

            string path =
                line.Substring(
                    prefix.Length,
                    line.Length -
                    prefix.Length -
                    suffix.Length)
                .Trim();

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new FormatException(
                    $"Image path must not be empty " +
                    $"at line {currentLine + 1}.");
            }

            currentLine++;

            return new ImageBlockData(path);
        }

        private ManualBlockData ParseNoteBlock(
            string[] lines,
            ref int currentLine)
        {
            currentLine++;

            var inlines =
                new List<ManualInlineData>();

            while (currentLine < lines.Length)
            {
                string line = lines[currentLine];

                if (line == "[/note]")
                {
                    currentLine++;

                    return new NoteBlockData(inlines);
                }

                if (!string.IsNullOrEmpty(line))
                {
                    if (inlines.Count > 0)
                    {
                        inlines.Add(
                            new TextInlineData("\n"));
                    }

                    inlines.AddRange(
                        inlineParser.Parse(line));
                }

                currentLine++;
            }

            throw new FormatException(
                "Note block was not closed with [/note].");
        }

        private ManualBlockData ParseTextBlock(
            string[] lines,
            ref int currentLine)
        {
            var textLines =
                new List<string>();

            while (currentLine < lines.Length)
            {
                string line = lines[currentLine];

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                if (line == "---" ||
                    line == "[blank]" ||
                    line == "[note]" ||
                    line.StartsWith("[image:") ||
                    line.StartsWith("#"))
                {
                    break;
                }

                textLines.Add(line);
                currentLine++;
            }

            string text =
                string.Join("\n", textLines);

            return new TextBlockData(
                inlineParser.Parse(text));
        }

        
    }
}