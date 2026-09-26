using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialoguePlaybackDebugTest : MonoBehaviour
    {
        [SerializeField] private TextAsset dialogueCsv;

        private void Start()
        {
            RunTest();
        }

        private void RunTest()
        {
            if (dialogueCsv == null)
            {
                Debug.LogError(
                    "Dialogue CSV is not assigned.",
                    this);

                return;
            }

            try
            {
                var parser = new DialogueCsvParser();

                IReadOnlyList<DialogueLine> lines =
                    parser.Parse(dialogueCsv.text);

                var repository =
                    new DialogueRepository(lines);

                var playback =
                    new DialoguePlayback(repository);

                TestStart(playback);
                TestCharacterProgress(playback);
                TestLineProgress(playback);
                TestFinalLine(playback);
                TestStop(playback);

                Debug.Log(
                    "Dialogue playback test completed successfully.",
                    this);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
            }
        }

        private static void TestStart(
            DialoguePlayback playback)
        {
            bool started =
                playback.Start("customer_normal");

            if (!started)
            {
                throw new InvalidOperationException(
                    "Dialogue failed to start.");
            }

            if (!playback.IsPlaying)
            {
                throw new InvalidOperationException(
                    "Playback should be active after Start.");
            }

            if (playback.CurrentDialogueId !=
                "customer_normal")
            {
                throw new InvalidOperationException(
                    "Current dialogue ID is incorrect.");
            }

            if (playback.CurrentLineIndex != 0)
            {
                throw new InvalidOperationException(
                    "Playback should start at line index 0.");
            }

            if (playback.CurrentCharacterIndex != 0)
            {
                throw new InvalidOperationException(
                    "Playback should start at character index 0.");
            }

            Debug.Log(
                "Start test passed.");
        }

        private static void TestCharacterProgress(
            DialoguePlayback playback)
        {
            DialogueLine line =
                playback.CurrentLine;

            int textLength =
                line.Text.Length;

            for (int i = 0; i < textLength; i++)
            {
                playback.AdvanceCharacter();

                int expectedIndex = i + 1;

                if (playback.CurrentCharacterIndex !=
                    expectedIndex)
                {
                    throw new InvalidOperationException(
                        $"Expected character index " +
                        $"{expectedIndex}, " +
                        $"but got " +
                        $"{playback.CurrentCharacterIndex}.");
                }
            }

            if (!playback.IsLineCompleted)
            {
                throw new InvalidOperationException(
                    "Line should be completed.");
            }

            Debug.Log(
                "Character progress test passed.");
        }

        private static void TestLineProgress(
            DialoguePlayback playback)
        {
            bool moved =
                playback.MoveToNextLine();

            if (!moved)
            {
                throw new InvalidOperationException(
                    "Playback failed to move to next line.");
            }

            if (playback.CurrentLineIndex != 1)
            {
                throw new InvalidOperationException(
                    "Playback did not move to line index 1.");
            }

            if (playback.CurrentCharacterIndex != 0)
            {
                throw new InvalidOperationException(
                    "Character index should reset to 0 " +
                    "when moving to the next line.");
            }

            if (playback.IsLineCompleted)
            {
                throw new InvalidOperationException(
                    "New line should not be completed.");
            }

            Debug.Log(
                "Line progress test passed.");
        }

        private static void TestFinalLine(
            DialoguePlayback playback)
        {
            playback.CompleteCurrentLine();

            bool moved =
                playback.MoveToNextLine();

            if (!moved)
            {
                throw new InvalidOperationException(
                    "Playback should move to the final line.");
            }

            if (playback.CurrentLineIndex != 2)
            {
                throw new InvalidOperationException(
                    "Playback did not move to final line.");
            }

            if (!playback.IsLastLine)
            {
                throw new InvalidOperationException(
                    "Playback should recognize the final line.");
            }

            Debug.Log(
                "Final line test passed.");
        }

        private static void TestStop(
            DialoguePlayback playback)
        {
            playback.Stop();

            if (playback.IsPlaying)
            {
                throw new InvalidOperationException(
                    "Playback should be inactive after Stop.");
            }

            if (playback.CurrentDialogueId != null)
            {
                throw new InvalidOperationException(
                    "Dialogue ID should be cleared after Stop.");
            }

            if (playback.CurrentLine != null)
            {
                throw new InvalidOperationException(
                    "Current line should be cleared after Stop.");
            }

            Debug.Log(
                "Stop test passed.");
        }
    }
}