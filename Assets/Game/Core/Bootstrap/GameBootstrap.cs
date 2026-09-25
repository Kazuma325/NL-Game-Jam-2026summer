using System;
using ShopGame.Adventure.Data;
using ShopGame.Adventure.Runtime;
using ShopGame.Core.Context;
using GameEventBus = ShopGame.Core.EventBus.EventBus;
using ShopGame.Interaction.Runtime;
using ShopGame.Loop;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using ShopGame.State.World;
using ShopGame.Manual.Runtime;
using UnityEngine;
using ShopGame.Browser.Runtime;

namespace ShopGame.Core.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private string initialRoomId = "Shop";

        [SerializeField]
        private RoomData[] roomData = Array.Empty<RoomData>();

        [SerializeField]
        private TextAsset[] manualSources = Array.Empty<TextAsset>();

        [SerializeField]
        private ManualImageEntry[] manualImages = Array.Empty<ManualImageEntry>();

        public GameContext Context { get; private set; }

        private void Awake()
        {
            var eventBus = new GameEventBus();
            var knowledge = new KnowledgeState(eventBus);
            var progress = new GameProgress(eventBus);
            var world = new WorldState(initialRoomId);
            var loop = new LoopManager(world, eventBus);
            var roomAccess = new RoomAccessChecker(knowledge, progress);
            var rooms = new RoomManager(world, eventBus, roomData, roomAccess);
            var navigation = new RoomNavigationController(rooms);
            var knowledgeSlot = new KnowledgeSlot(eventBus);
            var knowledgeInteraction = new KnowledgeInteractionController(knowledgeSlot);
            var knowledgeSelection = new KnowledgeSelectionController(knowledgeSlot);
            var manualParser = new ManualParser();
            var manualPages = new ManualPageRepository(manualParser, manualSources);
            var manualImageRepository = new ManualImageRepository(manualImages);
            var browserManager = new BrowserManager(eventBus);
            var manualLinkInteraction = new ManualLinkInteractionController(browserManager, knowledgeSelection);
            var manualSearch = new ManualSearchManager(manualPages);
            var craftingStation = new CraftingStation(eventBus);

            Context = new GameContext(
                eventBus,
                knowledge,
                progress,
                world,
                loop,
                rooms,
                roomAccess,
                navigation,
                knowledgeSlot,
                knowledgeInteraction,
                knowledgeSelection,
                manualPages,
                manualImageRepository,
                browserManager,
                manualLinkInteraction,
                manualSearch,
                craftingStation);
        }
    }
}
