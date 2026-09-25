using System;
using System.Collections.Generic;
using ShopGame.Manual.Data;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualInlineParser
    {
        public IReadOnlyList<ManualInlineData> Parse(
            string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var result =
                new List<ManualInlineData>();

            int currentIndex = 0;

            while (currentIndex < text.Length)
            {
                int nextTokenIndex =
                    FindNextInlineToken(
                        text,
                        currentIndex);

                if (nextTokenIndex < 0)
                {
                    AddTextInline(
                        result,
                        text.Substring(currentIndex));

                    break;
                }

                if (nextTokenIndex > currentIndex)
                {
                    AddTextInline(
                        result,
                        text.Substring(
                            currentIndex,
                            nextTokenIndex - currentIndex));
                }

                if (nextTokenIndex + 1 < text.Length &&
                    text[nextTokenIndex] == '[' &&
                    text[nextTokenIndex + 1] == '[')
                {
                    int linkEnd =
                        text.IndexOf(
                            "]]",
                            nextTokenIndex + 2,
                            StringComparison.Ordinal);

                    if (linkEnd < 0)
                    {
                        throw new FormatException(
                            "Link was not closed with ']]'.");
                    }

                    string linkContent =
                        text.Substring(
                            nextTokenIndex + 2,
                            linkEnd -
                            (nextTokenIndex + 2));

                    result.Add(
                        ParseLinkInline(linkContent));

                    currentIndex =
                        linkEnd + 2;

                    continue;
                }

                ParseStyleInline(
                    text,
                    nextTokenIndex,
                    result,
                    out int styleEndIndex);

                currentIndex = styleEndIndex;
            }

            return result;
        }

        private static int FindNextInlineToken(
            string text,
            int startIndex)
        {
            int linkIndex =
                text.IndexOf(
                    "[[",
                    startIndex,
                    StringComparison.Ordinal);

            int styleIndex =
                text.IndexOf(
                    "[style=",
                    startIndex,
                    StringComparison.Ordinal);

            if (linkIndex < 0)
                return styleIndex;

            if (styleIndex < 0)
                return linkIndex;

            return Math.Min(
                linkIndex,
                styleIndex);
        }

        private static LinkInlineData ParseLinkInline(
            string linkContent)
        {
            string[] parts =
                linkContent.Split('|');

            if (parts.Length < 2 ||
                parts.Length > 3)
            {
                throw new FormatException(
                    "Link must use the format " +
                    "[[displayText|pageId]] or " +
                    "[[displayText|pageId|knowledgeId]].");
            }

            string displayText =
                parts[0].Trim();

            string pageId =
                parts[1].Trim();

            string knowledgeId =
                pageId;

            if (parts.Length == 3)
            {
                knowledgeId =
                    parts[2].Trim();

                if (knowledgeId == "none")
                    knowledgeId = null;
            }

            return new LinkInlineData(
                displayText,
                pageId,
                knowledgeId);
        }

        private static void ParseStyleInline(
            string text,
            int styleStartIndex,
            List<ManualInlineData> result,
            out int nextIndex)
        {
            const string stylePrefix = "[style=";

            int stylePrefixEnd =
                styleStartIndex +
                stylePrefix.Length;

            int styleNameEnd =
                text.IndexOf(
                    "]",
                    stylePrefixEnd,
                    StringComparison.Ordinal);

            if (styleNameEnd < 0)
            {
                throw new FormatException(
                    "Style opening tag was not closed with ']'.");
            }

            string styleId =
                text.Substring(
                    stylePrefixEnd,
                    styleNameEnd - stylePrefixEnd)
                .Trim();

            if (string.IsNullOrWhiteSpace(styleId))
            {
                throw new FormatException(
                    "Style ID must not be empty.");
            }

            const string styleSuffix = "[/style]";

            int styleEnd =
                text.IndexOf(
                    styleSuffix,
                    styleNameEnd + 1,
                    StringComparison.Ordinal);

            if (styleEnd < 0)
            {
                throw new FormatException(
                    "Style block was not closed with [/style].");
            }

            string styledText =
                text.Substring(
                    styleNameEnd + 1,
                    styleEnd - (styleNameEnd + 1));

            if (string.IsNullOrEmpty(styledText))
            {
                throw new FormatException(
                    "Styled text must not be empty.");
            }

            result.Add(
                new StyledTextInlineData(
                    styledText,
                    styleId));

            nextIndex =
                styleEnd + styleSuffix.Length;
        }

        private static void AddTextInline(
            List<ManualInlineData> result,
            string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            result.Add(
                new TextInlineData(text));
        }
    }
}