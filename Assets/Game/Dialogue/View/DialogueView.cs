using System;
using TMPro;
using UnityEngine;
using ShopGame.Dialogue.Runtime;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueView : MonoBehaviour
    {
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private TMP_Text dialogueText;

        public void DisplayLine(
    DialogueLine line,
    int visibleCharacterCount)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));

            if (speakerNameText == null)
                throw new InvalidOperationException(
                    "Speaker name text is not assigned.");

            if (dialogueText == null)
                throw new InvalidOperationException(
                    "Dialogue text is not assigned.");

            if (visibleCharacterCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(visibleCharacterCount));

            speakerNameText.text =
                line.SpeakerName;

            int characterCount =
                Mathf.Min(
                    visibleCharacterCount,
                    line.Text.Length);

            dialogueText.text =
                line.Text.Substring(0, characterCount);
        }

        public void Clear()
        {
            if (speakerNameText != null)
                speakerNameText.text = string.Empty;

            if (dialogueText != null)
                dialogueText.text = string.Empty;
        }
    }
}