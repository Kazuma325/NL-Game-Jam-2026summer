namespace ShopGame.Core.EventBus
{
    public readonly struct DialogueEndedEvent
    {
        public string DialogueId { get; }

        public DialogueEndedEvent(string dialogueId)
        {
            DialogueId = dialogueId;
        }
    }
}