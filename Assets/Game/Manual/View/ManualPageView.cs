using ShopGame.Manual.Data;
using ShopGame.Manual.Runtime;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Manual.View
{
    public sealed class ManualPageView : MonoBehaviour
    {
        [SerializeField]
        private Transform contentRoot;

        [SerializeField]
        private TMP_FontAsset fontAsset;

        [SerializeField]
        private float heading1FontSize = 32f;

        [SerializeField]
        private float heading2FontSize = 26f;

        [SerializeField]
        private float heading3FontSize = 22f;

        [SerializeField]
        private float bodyFontSize = 18f;

        private ManualImageRepository imageRepository;

        private ManualLinkInteractionController linkInteraction;

        public void Initialize(ManualImageRepository imageRepository, ManualLinkInteractionController linkInteraction)
        {
            this.imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            this.linkInteraction = linkInteraction ?? throw new ArgumentNullException(nameof(linkInteraction));
        }

        public void DisplayPage(ManualPageData page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (contentRoot == null)
                throw new InvalidOperationException(
                    "Content Root is not assigned.");

            ClearContent();

            DisplayBlankLine();
            DisplayBlankLine();
            DisplayBlankLine();

            foreach (ManualBlockData block in page.Blocks)
            {
                if (block == null)
                    continue;

                DisplayBlock(block);
            }

            DisplayBlankLine();
            DisplayBlankLine();
            DisplayBlankLine();
        }

        private void DisplayBlock(ManualBlockData block)
        {
            if (block is HeadingBlockData heading)
            {
                DisplayHeading(heading);
                return;
            }

            if (block is TextBlockData text)
            {
                DisplayText(text);
                return;
            }

            if (block is ImageBlockData image)
            {
                DisplayImage(image);
                return;
            }

            if (block is NoteBlockData note)
            {
                DisplayNote(note);
                return;
            }

            if (block is SeparatorBlockData)
            {
                DisplaySeparator();
                return;
            }
            if (block is BlankLineBlockData)
            {
                DisplayBlankLine();
                return;
            }

            throw new InvalidOperationException(
                $"Unsupported manual block type: " +
                $"{block.GetType().Name}");
        }

        private void DisplayHeading(
    HeadingBlockData block)
        {
            ManualInlineTextView text =
                CreateInlineTextObject(contentRoot);

            text.Initialize(
                block.Inlines, linkInteraction.HandleLinkClicked);

            text.fontSize =
                GetHeadingFontSize(block.Level);

            text.fontStyle =
                FontStyles.Bold;
        }

        private void DisplayText(
    TextBlockData block)
        {
            ManualInlineTextView text =
                CreateInlineTextObject(contentRoot);

            text.Initialize(
                block.Inlines, linkInteraction.HandleLinkClicked);

            text.fontSize =
                bodyFontSize;
        }

        private void DisplayImage(
    ImageBlockData block)
        {
            if (imageRepository == null)
            {
                throw new InvalidOperationException(
                    "ManualImageRepository is not initialized.");
            }

            GameObject imageObject =
                new GameObject(
                    "ManualImage",
                    typeof(RectTransform),
                    typeof(Image));

            imageObject.transform.SetParent(
                contentRoot,
                false);

            Image image =
                imageObject.GetComponent<Image>();

            image.sprite =
                imageRepository.GetImage(
                    block.ImagePath);

            image.preserveAspect =
                true;
        }

        private void DisplayNote(
    NoteBlockData block)
        {
            GameObject noteObject =
                new GameObject(
                    "ManualNote",
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(VerticalLayoutGroup),
                    typeof(ContentSizeFitter));

            noteObject.transform.SetParent(
                contentRoot,
                false);

            Image background =
                noteObject.GetComponent<Image>();

            background.color =
                new Color(0.92f, 0.92f, 0.92f, 1f);

            VerticalLayoutGroup layout =
                noteObject.GetComponent<VerticalLayoutGroup>();

            layout.padding =
                new RectOffset(
                    16,
                    16,
                    12,
                    12);

            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            ContentSizeFitter fitter =
                noteObject.GetComponent<ContentSizeFitter>();

            fitter.horizontalFit =
                ContentSizeFitter.FitMode.Unconstrained;

            fitter.verticalFit =
                ContentSizeFitter.FitMode.PreferredSize;

            ManualInlineTextView text =
    CreateInlineTextObject(
        noteObject.transform);

            text.Initialize(
                block.Inlines, linkInteraction.HandleLinkClicked);

            text.fontSize =
                bodyFontSize;
        }

        private void DisplaySeparator()
        {
            GameObject separatorObject =
                new GameObject(
                    "ManualSeparator",
                    typeof(RectTransform),
                    typeof(UnityEngine.UI.Image),
                    typeof(UnityEngine.UI.LayoutElement));

            separatorObject.transform.SetParent(
                contentRoot,
                false);

            UnityEngine.UI.Image image =
                separatorObject.GetComponent<UnityEngine.UI.Image>();

            UnityEngine.UI.LayoutElement layoutElement =
    separatorObject.GetComponent<UnityEngine.UI.LayoutElement>();

            layoutElement.minHeight = 1f;
            layoutElement.preferredHeight = 1f;

            image.color = Color.gray;

            RectTransform rectTransform =
                image.rectTransform;

            rectTransform.anchorMin =
                new Vector2(0f, 1f);

            rectTransform.anchorMax =
                new Vector2(1f, 1f);

            rectTransform.pivot =
                new Vector2(0.5f, 1f);

            rectTransform.sizeDelta =
                new Vector2(0f, 1f);
        }

        private void DisplayBlankLine()
        {
            GameObject blankLineObject =
                new GameObject(
                    "ManualBlankLine",
                    typeof(RectTransform),
                    typeof(LayoutElement));

            blankLineObject.transform.SetParent(
                contentRoot,
                false);

            LayoutElement layoutElement =
                blankLineObject.GetComponent<LayoutElement>();

            layoutElement.preferredHeight =
                bodyFontSize;
        }

        private TMP_Text CreateTextObject(Transform parent)
        {
            GameObject textObject = new GameObject("ManualText", typeof(RectTransform), typeof(TextMeshProUGUI));

            textObject.transform.SetParent(parent, false);

            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

            if (fontAsset != null) text.font = fontAsset;

            text.textWrappingMode = TextWrappingModes.Normal;
            text.alignment = TextAlignmentOptions.Left;
            text.color = Color.black;

            RectTransform rectTransform = text.rectTransform;
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;

            return text;
        }

        private float GetHeadingFontSize(int level)
        {
            switch (level)
            {
                case 1:
                    return heading1FontSize;

                case 2:
                    return heading2FontSize;

                case 3:
                    return heading3FontSize;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(level),
                        level,
                        "Unsupported heading level.");
            }
        }

        private void ApplyStyle(
    TMP_Text text,
    string styleId)
        {
            switch (styleId)
            {
                case "important":
                    text.fontStyle =
                        FontStyles.Bold;
                    break;

                case "warning":
                    text.fontStyle =
                        FontStyles.Bold;
                    break;

                case "small":
                    text.fontSize *= 0.8f;
                    break;

                default:
                    Debug.LogWarning(
                        $"Unknown manual style: {styleId}");
                    break;
            }
        }

        private ManualInlineTextView CreateInlineTextObject(
    Transform parent)
        {
            GameObject textObject =
                new GameObject(
                    "ManualText",
                    typeof(RectTransform),
                    typeof(ManualInlineTextView));

            textObject.transform.SetParent(
                parent,
                false);

            ManualInlineTextView text =
                textObject.GetComponent<ManualInlineTextView>();

            if (fontAsset != null)
            {
                text.font =
                    fontAsset;
            }

            text.textWrappingMode = TextWrappingModes.Normal;

            text.alignment =
                TextAlignmentOptions.Left;

            text.color =
                Color.black;

            RectTransform rectTransform =
                text.rectTransform;

            rectTransform.anchorMin =
                new Vector2(0f, 1f);

            rectTransform.anchorMax =
                new Vector2(1f, 1f);

            rectTransform.pivot =
                new Vector2(0.5f, 1f);

            rectTransform.offsetMin =
                Vector2.zero;

            rectTransform.offsetMax =
                Vector2.zero;

            rectTransform.sizeDelta =
                Vector2.zero;

            return text;
        }

        private void ClearContent()
        {
            for (int i = contentRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(contentRoot.GetChild(i).gameObject);
            }
        }
    }
}