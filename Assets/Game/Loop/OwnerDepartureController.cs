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

            HandleOwnerDeparture();
        }

        private void HandleOwnerDeparture()
        {
            ownerState.Depart();

            string dialogueId =
                loopManager.LoopCount == 1
                    ? FirstLoopCustomerDialogueId
                    : CustomerDialogueId;

            if (dialogueManager.IsPlaying)
            {
                pendingDialogueId = dialogueId;
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

            if (string.IsNullOrWhiteSpace(pendingDialogueId))
            {
                return;
            }

            string dialogueId = pendingDialogueId;
            pendingDialogueId = null;

            StartCustomerDialogue(dialogueId);
        }
    }
}