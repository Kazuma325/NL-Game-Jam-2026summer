using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Runtime;
using ShopGame.State.World;
using System;

namespace ShopGame.Loop
{
    public sealed class OwnerDepartureController
    {
        private const string OwnerDepartureEventId =
            "owner_departure";

        private const string FirstLoopCustomerDialogueId =
            "customer_lp0";

        private const string CustomerDialogueId =
            "customer";

        private const float DialogueTransitionDelaySeconds = 0.2f;

        private float dialogueTransitionTimer;
        private bool isWaitingForDialogueTransition;

        private readonly OwnerState ownerState;
        private readonly CustomerState customerState;
        private readonly DialogueManager dialogueManager;
        private readonly LoopManager loopManager;
        private readonly EventBus eventBus;

        private string pendingDialogueId;

        public OwnerDepartureController(
            OwnerState ownerState,
            CustomerState customerState,
            DialogueManager dialogueManager,
            LoopManager loopManager,
            EventBus eventBus)
        {
            this.ownerState =
                ownerState
                ?? throw new ArgumentNullException(nameof(ownerState));

            this.customerState =
                customerState
                ?? throw new ArgumentNullException(nameof(customerState));

            this.dialogueManager =
                dialogueManager
                ?? throw new ArgumentNullException(nameof(dialogueManager));

            this.loopManager =
                loopManager
                ?? throw new ArgumentNullException(nameof(loopManager));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            eventBus.Subscribe<LoopEventTriggeredEvent>(
                OnLoopEventTriggered);

            eventBus.Subscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        public void Update(float deltaTime)
        {
            if (string.IsNullOrWhiteSpace(pendingDialogueId))
            {
                return;
            }

            if (dialogueManager.IsPlaying)
            {
                return;
            }

            if (!isWaitingForDialogueTransition)
            {
                isWaitingForDialogueTransition = true;
                dialogueTransitionTimer = DialogueTransitionDelaySeconds;
                return;
            }

            dialogueTransitionTimer -= deltaTime;

            if (dialogueTransitionTimer > 0f)
            {
                return;
            }

            isWaitingForDialogueTransition = false;

            string dialogueId = pendingDialogueId;
            pendingDialogueId = null;

            StartCustomerDialogue(dialogueId);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<LoopEventTriggeredEvent>(
                OnLoopEventTriggered);

            eventBus.Unsubscribe<DialogueEndedEvent>(
                OnDialogueEnded);
        }

        private void OnLoopEventTriggered(
    LoopEventTriggeredEvent eventData)
        {
            if (eventData.EventId != OwnerDepartureEventId)
            {
                return;
            }

            UnityEngine.Debug.Log(
                "OwnerDepartureController: owner_departure triggered.");

            HandleOwnerDeparture();
        }

        private void HandleOwnerDeparture()
        {
            ownerState.Depart();

            string dialogueId =
                loopManager.LoopCount == 1
                    ? FirstLoopCustomerDialogueId
                    : CustomerDialogueId;

            UnityEngine.Debug.Log(
                $"OwnerDepartureController: target dialogue = {dialogueId}");

            UnityEngine.Debug.Log(
                $"OwnerDepartureController: IsPlaying = {dialogueManager.IsPlaying}");

            if (dialogueManager.IsPlaying)
            {
                pendingDialogueId = dialogueId;

                UnityEngine.Debug.Log(
                    $"OwnerDepartureController: dialogue pending = {pendingDialogueId}");

                return;
            }

            StartCustomerDialogue(dialogueId);
        }

        private void StartCustomerDialogue(
            string dialogueId)
        {
            bool started =
                dialogueManager.StartDialogue(dialogueId);

            if (!started)
            {
                throw new InvalidOperationException(
                    $"Failed to start dialogue '{dialogueId}'.");
            }
        }

        private void OnDialogueEnded(
    DialogueEndedEvent eventData)
        {
            if (eventData.DialogueId == CustomerDialogueId ||
                eventData.DialogueId == FirstLoopCustomerDialogueId)
            {
                customerState.Appear();
            }
        }
    }
}