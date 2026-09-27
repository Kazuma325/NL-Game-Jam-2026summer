using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Dialogue.View
{
    public sealed class DialoguePortraitView : MonoBehaviour
    {
        [SerializeField] private RectTransform portraitContainer;
        [SerializeField] private Image portraitImage;

        private GameObject currentPrefab;
        private string currentPortraitId;

        public void Display(
            string portraitId,
            DialoguePortraitRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            if (string.IsNullOrWhiteSpace(portraitId))
            {
                Clear();
                return;
            }

            // 同じPortraitIdなら現在の表示を維持する。
            if (currentPortraitId == portraitId)
            {
                return;
            }

            if (!repository.TryGet(
                    portraitId,
                    out DialoguePortraitEntry entry))
            {
                Clear();
                return;
            }

            Clear();

            switch (entry.Type)
            {
                case DialoguePortraitType.Sprite:
                    DisplaySprite(entry);
                    break;

                case DialoguePortraitType.Prefab:
                    DisplayPrefab(entry);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            currentPortraitId = portraitId;
        }

        public void Clear()
        {
            if (currentPrefab != null)
            {
                Destroy(currentPrefab);
                currentPrefab = null;
            }

            if (portraitImage != null)
            {
                portraitImage.sprite = null;
                portraitImage.enabled = false;
            }

            currentPortraitId = null;
        }

        private void DisplaySprite(
            DialoguePortraitEntry entry)
        {
            if (portraitImage == null)
            {
                throw new InvalidOperationException(
                    "Portrait Image is not assigned.");
            }

            portraitImage.sprite = entry.Sprite;
            portraitImage.enabled = true;
        }

        private void DisplayPrefab(
            DialoguePortraitEntry entry)
        {
            if (portraitContainer == null)
            {
                throw new InvalidOperationException(
                    "Portrait Container is not assigned.");
            }

            currentPrefab =
                Instantiate(
                    entry.Prefab,
                    portraitContainer);

            RectTransform rectTransform =
                currentPrefab.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                rectTransform.localScale = Vector3.one;
                rectTransform.localRotation = Quaternion.identity;
            }
        }
    }
}