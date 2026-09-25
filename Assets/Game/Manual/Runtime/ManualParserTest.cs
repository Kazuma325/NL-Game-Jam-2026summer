using ShopGame.Core.Bootstrap;
using ShopGame.Manual.Data;
using ShopGame.Manual.View;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualParserTest : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private ManualPageView manualPageView;

        private void Start()
        {
            if (gameBootstrap == null)
            {
                Debug.LogError(
                    "GameBootstrap is not assigned.");

                return;
            }

            if (manualPageView == null)
            {
                Debug.LogError(
                    "ManualPageView is not assigned.");

                return;
            }

            ManualPageData page =
                gameBootstrap.Context.ManualPages
                    .GetPage("telephone");

            Debug.Log(
                $"Context Manual Page: " +
                $"{page.Id} / {page.Title}");

            Debug.Log(
    $"Keywords: " +
    $"{string.Join(", ", page.Keywords)}");

            Debug.Log(
                $"Block count: " +
                $"{page.Blocks.Count}");

            manualPageView.Initialize(gameBootstrap.Context.ManualImages, gameBootstrap.Context.ManualLinks);
            manualPageView.DisplayPage(page);

            IReadOnlyList<ManualPageData> results =
                gameBootstrap.Context.ManualSearch.Search("電話");

            foreach (ManualPageData result in results)
            {
                Debug.Log(
                    $"Search Result: {result.Id} / {result.Title}");
            }
        }
    }
}