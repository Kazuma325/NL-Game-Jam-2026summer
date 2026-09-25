using ShopGame.Browser.Runtime;
using ShopGame.Core.Bootstrap;
using UnityEngine;

namespace ShopGame.Browser.View
{
    public sealed class BrowserManagerDebugTest : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        private void Start()
        {
            var browser =
                gameBootstrap.Context.Browser;

            Debug.Log(
                $"Tabs: {browser.Tabs.Count}");

            Debug.Log(
                $"Active: {browser.ActiveTab.PageId}");

            browser.OpenPage("telephone");

            Debug.Log(
                $"After OpenPage: {browser.ActiveTab.PageId}");

            Debug.Log(
                $"After NewTab: " +
                $"Tabs={browser.Tabs.Count}, " +
                $"Active={browser.ActiveTab.PageId}");

            browser.ActivateTab(1);

            Debug.Log(
                $"After ActivateTab: " +
                $"Active={browser.ActiveTab.PageId}");

            browser.CloseTab(1);

            Debug.Log(
                $"After CloseTab: " +
                $"Tabs={browser.Tabs.Count}, " +
                $"Active={browser.ActiveTab.PageId}");

            browser.OpenPage("telephone");

            browser.OpenPage("telephone");
            browser.OpenPage("phoneNumber");

            Debug.Log(
                $"Current: {browser.ActiveTab.PageId}");

            browser.GoBack();

            Debug.Log(
                $"After Back: {browser.ActiveTab.PageId}");
        }
    }
}