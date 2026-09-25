namespace ShopGame.Core.EventBus
{
    public readonly struct KnowledgeAddedEvent
    {
        public string KnowledgeId { get; }

        public KnowledgeAddedEvent(string knowledgeId)
        {
            KnowledgeId = knowledgeId;
        }
    }

    public readonly struct ProgressCompletedEvent
    {
        public string ProgressId { get; }

        public ProgressCompletedEvent(string progressId)
        {
            ProgressId = progressId;
        }
    }

    public readonly struct RoomChangedEvent
    {
        public string PreviousRoomId { get; }
        public string CurrentRoomId { get; }

        public RoomChangedEvent(string previousRoomId, string currentRoomId)
        {
            PreviousRoomId = previousRoomId;
            CurrentRoomId = currentRoomId;
        }
    }

    public readonly struct LoopStartedEvent
    {
        public int LoopCount { get; }

        public LoopStartedEvent(int loopCount)
        {
            LoopCount = loopCount;
        }
    }

    public readonly struct LoopEndedEvent
    {
        public int LoopCount { get; }

        public LoopEndedEvent(int loopCount)
        {
            LoopCount = loopCount;
        }
    }

    public readonly struct GameOverEvent
    {
        public string ReasonId { get; }

        public GameOverEvent(string reasonId)
        {
            ReasonId = reasonId;
        }
    }

    public readonly struct BrowserChangedEvent
    {
    }

    public readonly struct KnowledgeSlotChangedEvent
    {
    }
}
