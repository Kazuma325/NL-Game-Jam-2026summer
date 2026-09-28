using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Browser.View
{
    public sealed class PCInteractionView : MonoBehaviour
    {
        [SerializeField]
        private Button pcButton;

        [SerializeField]
        private GameObject browserPanel;

        private void Awake()
        {
            if (pcButton == null)
            {
                throw new InvalidOperationException(
                    "PC Button is not assigned.");
            }

            if (browserPanel == null)
            {
                throw new InvalidOperationException(
                    "Browser Panel is not assigned.");
            }

            browserPanel.SetActive(false);

            pcButton.onClick.AddListener(
                OpenBrowser);
        }

        private void OnDestroy()
        {
            if (pcButton == null)
            {
                return;
            }

            pcButton.onClick.RemoveListener(
                OpenBrowser);
        }

        private void OpenBrowser()
        {
            browserPanel.SetActive(true);
        }
    }
}