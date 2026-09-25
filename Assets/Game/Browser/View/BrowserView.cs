using ShopGame.Browser.Runtime;
using ShopGame.Core.Bootstrap;
using ShopGame.Core.EventBus;
using ShopGame.Manual.Data;
using ShopGame.Manual.View;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Browser.View
{
    public sealed class BrowserView : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private Transform tabRoot;

        [SerializeField]
        private BrowserTabView tabPrefab;

        [SerializeField]
        private Manual.View.ManualPageView pageView;

        [SerializeField]
        private GameObject searchPageObj;

        [SerializeField]
        private ManualSearchPageView searchPageView;

        [SerializeField]
        private Button backButton;

        private readonly Dictionary<int, BrowserTabView>
            tabViews = new();

        private void Start()
        {
            if (gameBootstrap == null)
                throw new InvalidOperationException(
                    "GameBootstrap is not assigned.");

            if (tabRoot == null)
                throw new InvalidOperationException(
                    "Tab root is not assigned.");

            if (tabPrefab == null)
                throw new InvalidOperationException(
                    "Tab prefab is not assigned.");

            if (pageView == null)
                throw new InvalidOperationException(
                    "ManualPageView is not assigned.");

            gameBootstrap.Context.EventBus.Subscribe<BrowserChangedEvent>(
                HandleBrowserChanged);

            searchPageView.Initialize(
    gameBootstrap.Context.ManualSearch,
    gameBootstrap.Context.Browser);

            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(
                HandleBackButtonClicked);

            Refresh();
        }

        private void HandleBrowserChanged(
    BrowserChangedEvent eventData)
        {
            Refresh();
        }

        private void OnDestroy()
        {
            if (gameBootstrap == null)
                return;

            gameBootstrap.Context.EventBus
                .Unsubscribe<BrowserChangedEvent>(
                    HandleBrowserChanged);
        }

        private void Refresh()
        {
            BrowserManager browser =
                gameBootstrap.Context.Browser;

            foreach (Transform child in tabRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (BrowserTab tab in browser.Tabs)
            {
                BrowserTabView tabView =
                    Instantiate(tabPrefab, tabRoot);

                string displayName;

                if (tab.IsSearchTab)
                {
                    displayName = "検索";
                }
                else
                {
                    ManualPageData page =
                        gameBootstrap.Context.ManualPages
                            .GetPage(tab.PageId);

                    displayName = page.Title;
                }

                tabView.Initialize(
                    tab.TabId,
                    displayName,
                    !tab.IsSearchTab,
                    HandleTabSelected,
                    HandleTabClosed);

                tabView.SetActive(
                    tab.TabId == browser.ActiveTab.TabId);
            }

            DisplayActivePage();

            backButton.interactable =
    browser.ActiveTab.CanGoBack;
        }

        private void CreateTabView(
            BrowserTab tab,
            int activeTabId)
        {
            BrowserTabView view =
                Instantiate(
                    tabPrefab,
                    tabRoot);

            string displayName =
                GetDisplayName(tab);

            view.Initialize(
                tab.TabId,
                displayName,
                !tab.IsSearchTab,
                HandleTabSelected,
                HandleTabClosed);

            view.SetActive(
                tab.TabId == activeTabId);

            tabViews.Add(
                tab.TabId,
                view);
        }

        private void HandleTabSelected(int tabId)
        {
            gameBootstrap.Context.Browser
                .ActivateTab(tabId);

            Refresh();
        }

        private void HandleTabClosed(int tabId)
        {
            gameBootstrap.Context.Browser
                .CloseTab(tabId);

            Refresh();
        }

        private void DisplayActivePage()
        {
            BrowserTab activeTab =
                gameBootstrap.Context.Browser.ActiveTab;

            if (activeTab.IsSearchTab)
            {
                searchPageObj.SetActive(true);
                pageView.gameObject.SetActive(false);
                return;
            }

            searchPageObj.SetActive(false);
            pageView.gameObject.SetActive(true);

            var page =
                gameBootstrap.Context.ManualPages
                    .GetPage(activeTab.PageId);

            pageView.Initialize(
                gameBootstrap.Context.ManualImages, gameBootstrap.Context.ManualLinks);

            pageView.DisplayPage(page);
        }

        /*private void DisplaySearchPage()
        {
            pageView.Initialize(
                gameBootstrap.Context.ManualImages);

            pageView.DisplaySearchPage();
        }*/

        private static string GetDisplayName(
            BrowserTab tab)
        {
            if (tab.IsSearchTab)
                return "検索";

            return tab.PageId;
        }

        private void ClearTabViews()
        {
            foreach (Transform child in tabRoot)
            {
                Destroy(child.gameObject);
            }

            tabViews.Clear();
        }

        private void HandleBackButtonClicked()
        {
            gameBootstrap.Context.Browser.GoBack();
        }
    }
}