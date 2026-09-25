using ShopGame.Browser.Runtime;
using ShopGame.Manual.Data;
using ShopGame.Manual.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Manual.View
{
    public sealed class ManualSearchPageView : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField searchInput;

        [SerializeField]
        private Button searchButton;

        [SerializeField]
        private Transform resultsContent;

        [SerializeField]
        private ManualSearchResultView resultPrefab;

        private ManualSearchManager searchManager;
        private BrowserManager browser;

        public void Initialize(
            ManualSearchManager searchManager, BrowserManager browser)
        {
            this.searchManager =
                searchManager
                ?? throw new ArgumentNullException(
                    nameof(searchManager));
            this.browser =
                browser
                ?? throw new ArgumentNullException(
                    nameof(browser));

            if (searchInput == null)
            {
                throw new InvalidOperationException(
                    "Search input is not assigned.");
            }

            if (searchButton == null)
            {
                throw new InvalidOperationException(
                    "Search button is not assigned.");
            }

            if (resultsContent == null)
            {
                throw new InvalidOperationException(
                    "Results content is not assigned.");
            }

            if (resultPrefab == null)
            {
                throw new InvalidOperationException(
                    "Result prefab is not assigned.");
            }

            searchButton.onClick.RemoveAllListeners();
            searchButton.onClick.AddListener(
                HandleSearchClicked);
        }

        private void HandleSearchClicked()
        {
            string keyword =
                searchInput.text;

            IReadOnlyList<ManualPageData> results =
                searchManager.Search(keyword);

            DisplayResults(results);
        }

        private void DisplayResults(
            IReadOnlyList<ManualPageData> results)
        {
            ClearResults();

            foreach (ManualPageData page in results)
            {
                ManualSearchResultView resultView =
                    Instantiate(
                        resultPrefab,
                        resultsContent);

                resultView.Initialize(
                    page.Title,
                    () => HandleResultClicked(page.Id));
            }
        }

        private void HandleResultClicked(
    string pageId)
        {
            browser.OpenPage(pageId);
        }

        private void ClearResults()
        {
            for (int i = resultsContent.childCount - 1;
                 i >= 0;
                 i--)
            {
                Destroy(
                    resultsContent.GetChild(i).gameObject);
            }
        }
    }
}