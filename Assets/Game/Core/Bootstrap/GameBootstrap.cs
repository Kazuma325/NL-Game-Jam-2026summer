using NUnit.Framework;
using ShopGame.Adventure.Data;
using ShopGame.Adventure.Runtime;
using ShopGame.Browser.Runtime;
using ShopGame.Core.Context;
using ShopGame.Dialogue.Data;
using ShopGame.Dialogue.Runtime;
using ShopGame.Dialogue.View;
using ShopGame.Interaction.Data;
using ShopGame.Interaction.Runtime;
using ShopGame.Loop;
using ShopGame.Manual.Runtime;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using ShopGame.State.World;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameEventBus = ShopGame.Core.EventBus.EventBus;

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

        [SerializeField]
        private RecipeData[] recipeData = Array.Empty<RecipeData>();

        [SerializeField]
        private DialogueData[] dialogueData = Array.Empty<DialogueData>();

        [SerializeField]
        private TextAsset[] dialogueSources = Array.Empty<TextAsset>();

        public GameContext Context { get; private set; }

        [SerializeField]
        private DialoguePortraitEntry[] dialoguePortraitEntries = Array.Empty<DialoguePortraitEntry>();

        [SerializeField]
        private DialogueBackgroundEntry[] dialogueBackgroundEntries = Array.Empty<DialogueBackgroundEntry>();

        public DialoguePortraitRepository DialoguePortraitRepository
        {
            get;
            private set;
        }

        public DialogueBackgroundRepository DialogueBackgroundRepository
        {
            get;
            private set;
        }

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
            var dialogueParser = new DialogueCsvParser();
            var dialogueLines = new List<DialogueLine>();
            foreach (TextAsset dialogueSource in dialogueSources)
            {
                if(dialogueSource == null) continue;
                IReadOnlyList<DialogueLine> parsedLines = dialogueParser.Parse(dialogueSource.text);
                foreach(DialogueLine line in parsedLines) dialogueLines.Add(line);
            }
            var dialogueRepository = new DialogueRepository(dialogueLines);
            var dialoguePlayback = new DialoguePlayback(dialogueRepository);
            var dialogueTypewriter = new DialogueTypewriter(DialogueSpeed.Normal);
            var dialogueLog = new DialogueLog();
            var dialogueResolver = new DialogueResolver(dialogueData);
            var dialogueInteraction = new DialogueInteractionController(knowledgeSlot, dialogueResolver);
            DialoguePortraitRepository = new DialoguePortraitRepository(dialoguePortraitEntries);
            DialogueBackgroundRepository = new DialogueBackgroundRepository(dialogueBackgroundEntries);
            var dialogueManager = new DialogueManager(eventBus, dialoguePlayback, dialogueTypewriter, dialogueLog);
            var manualParser = new ManualParser();
            var manualPages = new ManualPageRepository(manualParser, manualSources);
            var manualImageRepository = new ManualImageRepository(manualImages);
            var browserManager = new BrowserManager(eventBus);
            var manualLinkInteraction = new ManualLinkInteractionController(browserManager, knowledgeSelection);
            var manualSearch = new ManualSearchManager(manualPages);
            var craftingStation = new CraftingStation(eventBus);
            var recipeMatcher = new RecipeMatcher(recipeData);
            var craftResultHandler = new CraftResultHandler(eventBus, recipeMatcher);

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
                dialogueResolver,
                dialogueInteraction,
                dialogueManager,
                manualPages,
                manualImageRepository,
                browserManager,
                manualLinkInteraction,
                manualSearch,
                craftingStation);
        }
    }
}
