using System;
using ShopGame.Browser.Runtime;
using ShopGame.Interaction.Runtime;
using UnityEngine.EventSystems;

namespace ShopGame.Manual.Runtime
{
    public sealed class ManualLinkInteractionController
    {
        private readonly BrowserManager browser;
        private readonly KnowledgeSelectionController knowledgeSelection;

        public ManualLinkInteractionController(
            BrowserManager browser,
            KnowledgeSelectionController knowledgeSelection)
        {
            this.browser =
                browser
                ?? throw new ArgumentNullException(nameof(browser));

            this.knowledgeSelection =
                knowledgeSelection
                ?? throw new ArgumentNullException(
                    nameof(knowledgeSelection));
        }

        public void HandleLinkClicked(
            string pageId,
            string knowledgeId,
            string displayText,
            PointerEventData eventData)
        {
            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            if (string.IsNullOrWhiteSpace(pageId))
                throw new ArgumentException(
                    "Page ID must not be null, empty, or whitespace.",
                    nameof(pageId));

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    HandleLeftClick(pageId);
                    break;

                case PointerEventData.InputButton.Right:
                    HandleRightClick(knowledgeId, displayText);
                    break;
            }
        }

        private void HandleLeftClick(
    string pageId)
        {
            bool ctrlPressed =
                UnityEngine.InputSystem.Keyboard.current != null
                && (
                    UnityEngine.InputSystem.Keyboard.current.leftCtrlKey.isPressed
                    || UnityEngine.InputSystem.Keyboard.current.rightCtrlKey.isPressed);

            if (ctrlPressed)
            {
                browser.OpenPageInNewTab(pageId);
                return;
            }

            browser.OpenPage(pageId);
        }

        private void HandleRightClick(
            string knowledgeId, string displayText)
        {
            if (string.IsNullOrWhiteSpace(knowledgeId))
                return;

            knowledgeSelection.SelectOrToggle(
                knowledgeId, displayText);
        }
    }
}