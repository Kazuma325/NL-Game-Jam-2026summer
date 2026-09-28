namespace ShopGame.Core.EventBus
{
    public readonly struct DialogueEndedEvent
    {
        public string DialogueId { get; }

        public bool EndLoopOnComplete { get; }

        public DialogueEndedEvent(
            string dialogueId,
            bool endLoopOnComplete)
        {
            DialogueId = dialogueId;
            EndLoopOnComplete = endLoopOnComplete;
        }
    }
}