using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Browser.View
{
    public sealed class BrowserTabView : MonoBehaviour
    {
        [SerializeField]
        private Button tabButton;

        [SerializeField]
        private TMP_Text label;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private Color activeColor, inactiveColor;

        public int TabId { get; private set; }

        private Action<int> onSelected;
        private Action<int> onClosed;

        public void Initialize(
            int tabId,
            string displayName,
            bool canClose,
            Action<int> onSelected,
            Action<int> onClosed)
        {
            if (tabButton == null)
                throw new InvalidOperationException(
                    "Tab button is not assigned.");

            if (label == null)
                throw new InvalidOperationException(
                    "Tab label is not assigned.");

            if (closeButton == null)
                throw new InvalidOperationException(
                    "Close button is not assigned.");

            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException(
                    "Tab display name must not be empty.",
                    nameof(displayName));

            this.TabId = tabId;
            this.onSelected = onSelected;
            this.onClosed = onClosed;

            label.text = displayName;

            closeButton.gameObject.SetActive(
                canClose);

            tabButton.onClick.RemoveAllListeners();
            tabButton.onClick.AddListener(
                HandleTabClicked);

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(
                HandleCloseClicked);
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                tabButton.image.color = activeColor;
            }
            else
            {
                tabButton.image.color = inactiveColor;
            }
        }

        private void HandleTabClicked()
        {
            onSelected?.Invoke(TabId);
        }

        private void HandleCloseClicked()
        {
            onClosed?.Invoke(TabId);
        }
    }
}