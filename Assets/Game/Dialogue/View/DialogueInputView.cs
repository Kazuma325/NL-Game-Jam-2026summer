using System;
using ShopGame.Core.Bootstrap;
using ShopGame.Dialogue.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueInputView : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrap gameBootstrap;

        [SerializeField]
        private DialogueLogView logView;

        public void OnDialogueClicked()
        {
            if (gameBootstrap == null)
                return;

            DialogueManager dialogueManager =
                gameBootstrap.Context.DialogueManager;

            if (!dialogueManager.IsPlaying)
                return;

            if (dialogueManager.IsPaused)
                return;

            if (!dialogueManager.IsLineCompleted)
            {
                dialogueManager.CompleteCurrentLine();
                return;
            }

            if (dialogueManager.IsLastLine)
            {
                dialogueManager.EndDialogue();
                return;
            }

            dialogueManager.MoveToNextLine();
        }

        public void OnSkipButtonClicked()
        {
            if (gameBootstrap == null)
                return;

            DialogueManager dialogueManager =
                gameBootstrap.Context.DialogueManager;

            if (!dialogueManager.IsPlaying)
                return;

            dialogueManager.Skip();
        }
    }
}