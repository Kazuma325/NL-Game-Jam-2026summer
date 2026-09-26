using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Dialogue.Runtime
{
    public sealed class DialogueRepositoryDebugTest : MonoBehaviour
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

                TestDialogue(
                    repository,
                    "customer_normal");

                TestDialogue(
                    repository,
                    "customer_desk");

                Debug.Log(
                    "Dialogue repository test completed successfully.",
                    this);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
            }
        }

        private static void TestDialogue(
            DialogueRepository repository,
            string dialogueId)
        {
            if (!repository.Contains(dialogueId))
            {
                throw new InvalidOperationException(
                    $"Dialogue '{dialogueId}' was not found.");
            }

            IReadOnlyList<DialogueLine> lines =
                repository.GetLines(dialogueId);

            Debug.Log(
                $"Dialogue '{dialogueId}' contains " +
                $"{lines.Count} lines.");

            foreach (DialogueLine line in lines)
            {
                Debug.Log(
                    $"[{line.DialogueId}] " +
                    $"Index={line.Index}, " +
                    $"Speaker={line.SpeakerName}, " +
                    $"Text={line.Text}, " +
                    $"Portrait={line.PortraitId}, " +
                    $"Background={line.BackgroundId}, " +
                    $"StopSkip={line.StopSkip}");
            }
        }
    }
}