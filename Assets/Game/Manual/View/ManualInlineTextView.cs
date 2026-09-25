using System;
using System.Collections.Generic;
using ShopGame.Manual.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShopGame.Manual.View
{
    public sealed class ManualInlineTextView :
        TextMeshProUGUI,
        IPointerClickHandler
    {
        private readonly List<LinkTarget> linkTargets = new();

        private Action<string, string, string, PointerEventData> onLinkClicked;

        public void Initialize(
            IReadOnlyList<ManualInlineData> inlines,
            Action<string, string, string, PointerEventData> onLinkClicked)
        {
            this.onLinkClicked = onLinkClicked ?? throw new ArgumentNullException(nameof(onLinkClicked));

            if (inlines == null)
                throw new ArgumentNullException(nameof(inlines));

            linkTargets.Clear();

            var builder =
                new System.Text.StringBuilder();

            foreach (ManualInlineData inline in inlines)
            {
                if (inline is TextInlineData text)
                {
                    builder.Append(
                        EscapeText(text.Text));

                    continue;
                }

                if (inline is LinkInlineData link)
                {
                    int linkIndex =
                        linkTargets.Count;

                    linkTargets.Add(
                        new LinkTarget(
                            link.PageId,
                            link.KnowledgeId,
                            link.DisplayText));

                    builder.Append(
                        $"<link=\"{linkIndex}\">");

                    builder.Append(
                        "<color=#0000EE><u>");

                    builder.Append(
                        EscapeText(link.DisplayText));

                    builder.Append(
                        "</u></color>");

                    builder.Append(
                        "</link>");

                    continue;
                }

                if (inline is StyledTextInlineData styled)
                {
                    builder.Append(
                        CreateStyledText(
                            styled));

                    continue;
                }

                throw new InvalidOperationException(
                    $"Unsupported inline type: " +
                    $"{inline.GetType().Name}");
            }

            text = builder.ToString();
        }

        public void OnPointerClick(
    PointerEventData eventData)
        {
            if (eventData == null)
                return;

            int linkIndex =
                TMP_TextUtilities.FindIntersectingLink(
                    this,
                    eventData.position,
                    eventData.pressEventCamera);

            if (linkIndex < 0)
                return;

            if (linkIndex >= linkTargets.Count)
            {
                Debug.LogError(
                    $"Manual link index is out of range: {linkIndex}");

                return;
            }

            LinkTarget target =
                linkTargets[linkIndex];

            onLinkClicked?.Invoke(
    target.PageId,
    target.KnowledgeId,
    target.DisplayText,
    eventData);
        }

        private static string CreateStyledText(
            StyledTextInlineData inline)
        {
            string text =
                EscapeText(inline.Text);

            switch (inline.StyleId)
            {
                case "important":
                    return $"<b>{text}</b>";

                case "warning":
                    return $"<b>{text}</b>";

                case "small":
                    return $"<size=80%>{text}</size>";

                default:
                    throw new InvalidOperationException(
                        $"Unknown manual style: " +
                        $"{inline.StyleId}");
            }
        }

        private static string EscapeText(
            string value)
        {
            return value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

        private readonly struct LinkTarget
        {
            public string PageId { get; }
            public string KnowledgeId { get; }
            public string DisplayText { get; }

            public LinkTarget(
                string pageId,
                string knowledgeId,
                string displayText)
            {
                PageId = pageId;
                KnowledgeId = knowledgeId;
                DisplayText = displayText;
            }
        }
    }
}