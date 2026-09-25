using System;
using UnityEngine;

namespace ShopGame.Manual.Data
{
    public sealed class LinkInlineData : ManualInlineData
    {
        public string DisplayText { get; }
        public string PageId { get; }
        public string KnowledgeId { get; }

        public LinkInlineData(string displayText, string pageId, string knowledgeId)
        {
            if (string.IsNullOrEmpty(displayText)) throw new ArgumentException("Display text must not be null, empty, or whitespace.", nameof(displayText));
            if(string.IsNullOrWhiteSpace(pageId)) throw new ArgumentException("Page ID must not be null, empty, or whitespace.", nameof(pageId));

            DisplayText = displayText;
            PageId = pageId;
            KnowledgeId = knowledgeId;
        }
    }
}