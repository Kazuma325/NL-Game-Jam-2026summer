using System;
using System.Collections.Generic;
using ShopGame.Core.EventBus;

namespace ShopGame.Browser.Runtime
{
    public sealed class BrowserManager
    {
        private readonly EventBus eventBus;
        private readonly List<BrowserTab> tabs = new();

        private int nextTabId;
        private int activeTabId;

        public IReadOnlyList<BrowserTab> Tabs => tabs;

        public BrowserTab ActiveTab
        {
            get
            {
                BrowserTab tab = FindTab(activeTabId);

                if (tab == null)
                {
                    throw new InvalidOperationException(
                        "Active browser tab was not found.");
                }

                return tab;
            }
        }

        public BrowserManager(EventBus eventBus)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            BrowserTab searchTab =
                new BrowserTab(
                    nextTabId++,
                    "search",
                    true);

            tabs.Add(searchTab);
            activeTabId = searchTab.TabId;
        }

        public void OpenPage(string pageId)
        {
            ValidatePageId(pageId);

            BrowserTab activeTab = ActiveTab;

            if (activeTab.IsSearchTab)
            {
                BrowserTab newTab =
                    CreateManualTab(pageId);

                activeTabId = newTab.TabId;

                eventBus.Publish(
                    new BrowserChangedEvent());

                return;
            }

            activeTab.NavigateTo(pageId);

            eventBus.Publish(
                new BrowserChangedEvent());
        }

        public void OpenPageInNewTab(string pageId)
        {
            ValidatePageId(pageId);

            BrowserTab newTab =
                CreateManualTab(pageId);

            activeTabId = newTab.TabId;

            eventBus.Publish(
                new BrowserChangedEvent());
        }

        public void CloseTab(int tabId)
        {
            BrowserTab tab = FindTab(tabId);

            if (tab == null)
            {
                throw new InvalidOperationException(
                    $"Browser tab was not found: {tabId}");
            }

            if (tab.IsSearchTab)
            {
                throw new InvalidOperationException(
                    "Search tab cannot be closed.");
            }

            int index = tabs.IndexOf(tab);

            tabs.RemoveAt(index);

            if (tab.TabId != activeTabId)
            {
                eventBus.Publish(
                    new BrowserChangedEvent());

                return;
            }

            if (tabs.Count == 0)
            {
                throw new InvalidOperationException(
                    "Browser must always contain the search tab.");
            }

            int newIndex =
                Math.Clamp(index - 1, 0, tabs.Count - 1);

            activeTabId =
                tabs[newIndex].TabId;

            eventBus.Publish(
                new BrowserChangedEvent());
        }

        public void ActivateTab(int tabId)
        {
            BrowserTab tab = FindTab(tabId);

            if (tab == null)
            {
                throw new InvalidOperationException(
                    $"Browser tab was not found: {tabId}");
            }

            activeTabId = tab.TabId;

            eventBus.Publish(
                new BrowserChangedEvent());
        }

        private BrowserTab CreateManualTab(string pageId)
        {
            BrowserTab tab =
                new BrowserTab(
                    nextTabId++,
                    pageId,
                    false);

            tabs.Add(tab);

            return tab;
        }

        private BrowserTab FindTab(int tabId)
        {
            foreach (BrowserTab tab in tabs)
            {
                if (tab.TabId == tabId)
                {
                    return tab;
                }
            }

            return null;
        }

        private static void ValidatePageId(string pageId)
        {
            if (string.IsNullOrWhiteSpace(pageId))
            {
                throw new ArgumentException(
                    "Page ID must not be null, empty, or whitespace.",
                    nameof(pageId));
            }
        }

        public bool GoBack()
        {
            BrowserTab activeTab = ActiveTab;

            if (!activeTab.CanGoBack)
            {
                return false;
            }

            bool wentBack =
                activeTab.GoBack();

            if (wentBack)
            {
                eventBus.Publish(
                    new BrowserChangedEvent());
            }

            return wentBack;
        }
    }
}