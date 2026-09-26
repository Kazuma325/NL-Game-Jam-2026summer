using UnityEngine;
using ShopGame.Core.Bootstrap;
using ShopGame.Dialogue.Runtime;

namespace ShopGame.Dialogue.DebugTest
{
    public sealed class DialogueResolverDebugTest : MonoBehaviour
    {
        [SerializeField] private GameBootstrap gameBootstrap;

        private void Start()
        {
            RunAllTests();

            TestDialogueManager();
        }

        private void RunAllTests()
        {
            Debug.Log("========== DialogueResolver Debug Test ==========");

            TestNormalDialogue();
            TestKnowledgeSpecificDialogue();
            TestProgressCondition();
            TestLoopCondition();
            TestRoomCondition();
            TestEventCondition();
            TestPriority();

            Debug.Log("========== DialogueResolver Debug Test Finished ==========");
        }

        private void TestNormalDialogue()
        {
            Debug.Log("----- Test: Normal Dialogue -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            DialogueResolutionResult result =
                Resolve("Customer");

            LogResult(
                "Empty slot",
                result);
        }

        private void TestKnowledgeSpecificDialogue()
        {
            Debug.Log("----- Test: Knowledge Specific -----");

            gameBootstrap.Context.KnowledgeSlot.SelectOrToggle(
                "Desk",
                "Desk");

            DialogueResolutionResult result =
                Resolve("Customer");

            LogResult(
                "Desk selected",
                result);

            gameBootstrap.Context.KnowledgeSlot.Clear();
        }

        private void TestProgressCondition()
        {
            Debug.Log("----- Test: Progress Condition -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            DialogueResolutionResult beforeResult =
                Resolve("Customer");

            LogResult(
                "Before DeskRequested",
                beforeResult);

            gameBootstrap.Context.Progress.CompleteProgress(
                "DeskRequested");

            DialogueResolutionResult afterResult =
                Resolve("Customer");

            LogResult(
                "After DeskRequested",
                afterResult);
        }

        private void TestLoopCondition()
        {
            Debug.Log("----- Test: Loop Condition -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            Debug.Log(
                $"Current LoopCount: {gameBootstrap.Context.Loop.LoopCount}");

            DialogueResolutionResult result =
                Resolve("Customer");

            LogResult(
                "Current loop",
                result);
        }

        private void TestRoomCondition()
        {
            Debug.Log("----- Test: Room Condition -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            Debug.Log(
                $"Current Room: {gameBootstrap.Context.World.CurrentRoomId}");

            DialogueResolutionResult result =
                Resolve("Customer");

            LogResult(
                "Current room",
                result);
        }

        private void TestEventCondition()
        {
            Debug.Log("----- Test: Event Condition -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            DialogueResolutionResult beforeResult =
                Resolve("Customer");

            LogResult(
                "Before CustomerWaiting",
                beforeResult);

            gameBootstrap.Context.World.Events.Activate(
                "CustomerWaiting");

            DialogueResolutionResult afterResult =
                Resolve("Customer");

            LogResult(
                "After CustomerWaiting",
                afterResult);

            gameBootstrap.Context.World.Events.Deactivate(
                "CustomerWaiting");
        }

        private void TestPriority()
        {
            Debug.Log("----- Test: Priority -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            DialogueResolutionResult result =
                Resolve("Customer");

            if (result == null)
            {
                Debug.Log(
                    "Priority Test: No dialogue matched.");
                return;
            }

            Debug.Log(
                $"Priority Test Result: DialogueId={result.DialogueId}");
        }

        private DialogueResolutionResult Resolve(string targetId)
        {
            return gameBootstrap.Context.DialogueInteraction
                .ResolveInteraction(
                    targetId,
                    gameBootstrap.Context);
        }

        private static void LogResult(
            string testName,
            DialogueResolutionResult result)
        {
            if (result == null)
            {
                Debug.Log(
                    $"{testName}: No dialogue matched.");
                return;
            }

            Debug.Log(
                $"{testName}: " +
                $"DialogueId={result.DialogueId}, " +
                $"ConsumeKnowledge={result.ConsumeKnowledge}");
        }

        private void TestDialogueManager()
        {
            Debug.Log("----- Test: DialogueManager -----");

            gameBootstrap.Context.KnowledgeSlot.Clear();

            DialogueResolutionResult result =
                gameBootstrap.Context.DialogueInteraction
                    .ResolveInteraction(
                        "Customer",
                        gameBootstrap.Context);

            if (result == null)
            {
                Debug.Log("Resolver returned null.");
                return;
            }

            DialogueManager manager =
                gameBootstrap.Context.DialogueManager;

            bool started =
                manager.StartDialogue(result);

            Debug.Log(
                $"StartDialogue: {started}, " +
                $"IsPlaying={manager.IsPlaying}, " +
                $"CurrentDialogueId={manager.CurrentDialogueId}");

            manager.EndDialogue();

            Debug.Log(
                $"After EndDialogue: " +
                $"IsPlaying={manager.IsPlaying}, " +
                $"CurrentDialogueId={manager.CurrentDialogueId}");
        }
    }
}