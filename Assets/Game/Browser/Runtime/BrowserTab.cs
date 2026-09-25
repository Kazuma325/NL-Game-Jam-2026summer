using System;
using System.Collections.Generic;

namespace ShopGame.Browser.Runtime
{
    public sealed class BrowserTab
    {
        private readonly Stack<string> backHistory = new();

        public int TabId { get; }

        public string PageId { get; private set; }

        public bool IsSearchTab { get; }

        public bool CanGoBack =>
            backHistory.Count > 0;

        public BrowserTab(
            int tabId,
            string pageId,
            bool isSearchTab)
        {
            if (tabId < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tabId));
            }

            if (string.IsNullOrWhiteSpace(pageId))
            {
                throw new ArgumentException(
                    "Page ID must not be null, empty, or whitespace.",
                    nameof(pageId));
            }

            TabId = tabId;
            PageId = pageId;
            IsSearchTab = isSearchTab;
        }

        public void NavigateTo(string pageId)
        {
            if (IsSearchTab)
            {
                throw new InvalidOperationException(
                    "Search tab cannot navigate to a manual page.");
            }

            if (string.IsNullOrWhiteSpace(pageId))
            {
                throw new ArgumentException(
                    "Page ID must not be null, empty, or whitespace.",
                    nameof(pageId));
            }

            if (PageId == pageId)
            {
                return;
            }

            backHistory.Push(PageId);
            PageId = pageId;
        }

        public bool GoBack()
        {
            if (!CanGoBack)
            {
                return false;
            }

            PageId = backHistory.Pop();

            return true;
        }
    }
}