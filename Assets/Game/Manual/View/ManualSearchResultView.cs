using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Manual.View
{
    public sealed class ManualSearchResultView : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private TMP_Text titleText;

        public void Initialize(
            string title,
            Action onClicked)
        {
            if (button == null)
            {
                throw new InvalidOperationException(
                    "Search result button is not assigned.");
            }

            if (titleText == null)
            {
                throw new InvalidOperationException(
                    "Search result title text is not assigned.");
            }

            if (onClicked == null)
            {
                throw new ArgumentNullException(
                    nameof(onClicked));
            }

            titleText.text = title;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(
                () => onClicked());
        }
    }
}