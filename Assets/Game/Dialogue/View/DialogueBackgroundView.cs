using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueBackgroundView : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;

        private string currentBackgroundId;

        public void Display(
            string backgroundId,
            DialogueBackgroundRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(backgroundId))
            {
                Clear();
                return;
            }

            if (currentBackgroundId == backgroundId)
            {
                return;
            }

            if (!repository.TryGet(
                    backgroundId,
                    out DialogueBackgroundEntry entry))
            {
                Clear();
                return;
            }

            backgroundImage.sprite = entry.Sprite;
            backgroundImage.enabled = true;

            currentBackgroundId = backgroundId;
        }

        public void Clear()
        {
            if (backgroundImage != null)
            {
                backgroundImage.sprite = null;
                backgroundImage.enabled = false;
            }

            currentBackgroundId = null;
        }
    }
}