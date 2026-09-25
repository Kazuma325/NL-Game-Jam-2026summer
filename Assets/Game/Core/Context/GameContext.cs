using System;
using ShopGame.Adventure.Runtime;
using GameEventBus = ShopGame.Core.EventBus.EventBus;
using ShopGame.Interaction.Runtime;
using ShopGame.Loop;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using ShopGame.State.World;
using ShopGame.Manual.Runtime;
using ShopGame.Browser.Runtime;

namespace ShopGame.Core.Context
{
    public sealed class GameContext
    {
        public GameEventBus EventBus { get; }

        public KnowledgeState Knowledge { get; }

        public GameProgress Progress { get; }

        public WorldState World { get; }

        public LoopManager Loop { get; }

        public RoomManager Rooms { get; }

        public RoomAccessChecker RoomAccess {  get; }

        public RoomNavigationController Navigation { get; }

        public KnowledgeSlot KnowledgeSlot { get; }

        public KnowledgeInteractionController KnowledgeInteraction { get; }

        public KnowledgeSelectionController KnowledgeSelection { get; }

        public ManualPageRepository ManualPages { get; }

        public ManualImageRepository ManualImages { get; }

        public BrowserManager Browser { get; }

        public ManualLinkInteractionController ManualLinks { get; }

        public ManualSearchManager ManualSearch { get; }

        public GameContext(
            GameEventBus eventBus,
            KnowledgeState knowledge,
            GameProgress progress,
            WorldState world,
            LoopManager loop,
            RoomManager rooms,
            RoomAccessChecker roomAccess,
            RoomNavigationController navigation,
            KnowledgeSlot knowledgeSlot,
            KnowledgeInteractionController knowledgeInteraction,
            KnowledgeSelectionController knowledgeSelection,
            ManualPageRepository manualPages,
            ManualImageRepository manualImages,
            BrowserManager browser,
            ManualLinkInteractionController manualLinks,
            ManualSearchManager manualSearch)
        {
            EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            Knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
            Progress = progress ?? throw new ArgumentNullException(nameof(progress));
            World = world ?? throw new ArgumentNullException(nameof(world));
            Loop = loop ?? throw new ArgumentNullException(nameof(loop));
            Rooms = rooms ?? throw new ArgumentNullException(nameof(rooms));
            RoomAccess = roomAccess ?? throw new ArgumentNullException(nameof(roomAccess));
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            KnowledgeSlot = knowledgeSlot ?? throw new ArgumentNullException(nameof(knowledgeSlot));
            KnowledgeInteraction = knowledgeInteraction ?? throw new ArgumentNullException(nameof(knowledgeInteraction));
            KnowledgeSelection = knowledgeSelection ?? throw new ArgumentNullException(nameof(knowledgeSelection));
            ManualPages = manualPages ?? throw new ArgumentNullException(nameof(manualPages));
            ManualImages = manualImages ?? throw new ArgumentNullException(nameof(manualImages));
            Browser = browser ?? throw new ArgumentNullException(nameof(browser));
            ManualLinks = manualLinks ?? throw new ArgumentNullException(nameof(manualLinks));
            ManualSearch = manualSearch ?? throw new ArgumentNullException(nameof(manualSearch));
        }
    }
}
