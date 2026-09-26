namespace ShopGame.Core.EventBus
{
    public readonly struct DialogueStartedEvent
    {
        public string DialogueId { get; }

        public DialogueStartedEvent(string dialogueId)
        {
            DialogueId = dialogueId;
        }
    }
}