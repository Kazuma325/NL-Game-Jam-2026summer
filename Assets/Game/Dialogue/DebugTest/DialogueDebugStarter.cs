using ShopGame.Core.Bootstrap;
using ShopGame.Dialogue.Runtime;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShopGame.Dialogue.View
{
    public sealed class DialogueDebugStarter : MonoBehaviour
    {
        [SerializeField] private GameBootstrap gameBootstrap;
        [SerializeField] private string dialogueId;

        private void Start()
        {
            if (gameBootstrap == null)
            {
                throw new InvalidOperationException(
                    "Game Bootstrap is not assigned.");
            }

            if (string.IsNullOrWhiteSpace(dialogueId))
            {
                throw new InvalidOperationException(
                    "Dialogue ID is not assigned.");
            }

            var resolutionResult =
                new DialogueResolutionResult(
                    dialogueId,
                    false);

            bool started =
                gameBootstrap.Context.DialogueManager.StartDialogue(
                    resolutionResult);

            if (!started)
            {
                Debug.LogError(
                    $"Failed to start dialogue '{dialogueId}'.");
            }
        }

        private void Update()
        {
            if (gameBootstrap == null)
                return;

            DialogueManager dialogueManager =
                gameBootstrap.Context.DialogueManager;

            if (!dialogueManager.IsPlaying)
                return;

            if (Keyboard.current == null)
                return;

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                dialogueManager.SetSpeed(
                    DialogueSpeed.Slow);
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                dialogueManager.SetSpeed(
                    DialogueSpeed.Normal);
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                dialogueManager.SetSpeed(
                    DialogueSpeed.Fast);
            }
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                dialogueManager.SetSpeed(
                    DialogueSpeed.Instant);
            }
        }
    }
}