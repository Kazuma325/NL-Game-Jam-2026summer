using ShopGame.Dialogue.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueLogView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text logText;

        [SerializeField]
        private ScrollRect scrollRect;

        public bool IsOpen =>
            gameObject.activeSelf;

        public void Open()
        {
            gameObject.SetActive(true);
            Debug.Log("opened");
        }

        public void Close()
        {
            gameObject.SetActive(false);
            Debug.Log("closed");
        }

        public void Display(
            IReadOnlyList<DialogueLogEntry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(
                    nameof(entries));
            }

            if (logText == null)
            {
                throw new InvalidOperationException(
                    "Log text is not assigned.");
            }

            if (entries.Count == 0)
            {
                logText.text = string.Empty;
                return;
            }

            var builder =
                new System.Text.StringBuilder();

            foreach (DialogueLogEntry entry in entries)
            {
                if (entry == null)
                    continue;

                if (!string.IsNullOrWhiteSpace(
                        entry.SpeakerName))
                {
                    builder.Append(
                        entry.SpeakerName);

                    builder.Append(
                        ": ");
                }

                builder.AppendLine(
                    entry.Text);

                builder.AppendLine();
            }

            logText.text =
                builder.ToString();

            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();

                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        public void Clear()
        {
            if (logText != null)
            {
                logText.text =
                    string.Empty;
            }
        }
    }
}