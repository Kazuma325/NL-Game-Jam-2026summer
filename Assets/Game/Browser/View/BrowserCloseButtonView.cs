using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Browser.View
{
    public sealed class BrowserCloseButtonView : MonoBehaviour
    {
        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private GameObject browserPanel;

        private void Awake()
        {
            if (closeButton == null)
            {
                throw new InvalidOperationException(
                    "Close Button is not assigned.");
            }

            if (browserPanel == null)
            {
                throw new InvalidOperationException(
                    "Browser Panel is not assigned.");
            }

            closeButton.onClick.AddListener(
                CloseBrowser);
        }

        private void OnDestroy()
        {
            if (closeButton == null)
            {
                return;
            }

            closeButton.onClick.RemoveListener(
                CloseBrowser);
        }

        private void CloseBrowser()
        {
            browserPanel.SetActive(false);
        }
    }
}