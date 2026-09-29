using ShopGame.Adventure.Data;
using ShopGame.Adventure.Runtime;
using ShopGame.Adventure.View;
using ShopGame.Browser.Runtime;
using ShopGame.Core.Context;
using ShopGame.Core.EventBus;
using ShopGame.Dialogue.Data;
using ShopGame.Dialogue.Runtime;
using ShopGame.Dialogue.View;
using ShopGame.Event.Runtime;
using ShopGame.Event.View;
using ShopGame.Interaction.Data;
using ShopGame.Interaction.Runtime;
using ShopGame.Interaction.View;
using ShopGame.Loop;
using ShopGame.Loop.Data;
using ShopGame.Manual.Runtime;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using ShopGame.State.World;
using ShopGame.Title.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using GameEventBus = ShopGame.Core.EventBus.EventBus;

namespace ShopGame.Core.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private float loopTimeLimitSeconds = 300f;

        [SerializeField]
        private LoopEventData[] loopEventData = Array.Empty<LoopEventData>();

        [SerializeField]
        private TextAsset interactionRulesText;

        [SerializeField]
        private RuleInteractionView ownerInteractionView;

        [SerializeField]
        private RuleInteractionView controlInteractionView;

        [SerializeField]
        private RuleInteractionView studyInteractionView;

        [SerializeField]
        private RuleInteractionView telephoneInteractionView;

        [SerializeField]
        private CustomerView customerView;

        [SerializeField]
        private RuleInteractionView customerInteractionView;

        [SerializeField]
        private DialogueCanvasController dialogueCanvasController;

        [SerializeField]
        private string initialRoomId = "Shop";

        [SerializeField]
        private RoomData[] roomData = Array.Empty<RoomData>();

        [SerializeField]
        private OwnerViewPresenter ownerViewPresenter;

        [SerializeField]
        private TextAsset[] manualSources = Array.Empty<TextAsset>();

        [SerializeField]
        private ManualImageEntry[] manualImages = Array.Empty<ManualImageEntry>();

        [SerializeField]
        private RecipeData[] recipeData = Array.Empty<RecipeData>();

        [SerializeField]
        private DialogueData[] dialogueData = Array.Empty<DialogueData>();

        [SerializeField]
        private string craftingDialogueTargetId = "blender";

        [SerializeField]
        private KnowledgeSlotDialogueTriggerData[] knowledgeSlotDialogueTriggerData = Array.Empty<KnowledgeSlotDialogueTriggerData>();

        [SerializeField]
        private TextAsset[] dialogueSources = Array.Empty<TextAsset>();

        public GameContext Context { get; private set; }

        private LoopRestartController loopRestart;
        private LoopEventScheduler loopEventScheduler;
        private OwnerDepartureController ownerDepartureController;
        private LoopDialogueController loopDialogueController;

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

        public KnowledgeSlotDialogueTrigger KnowledgeSlotDialogueTrigger
        {
            get;
            private set;
        }

        public CraftingDialogueTrigger CraftingDialogueTrigger
        {
            get;
            private set;
        }

        public RuleInteractionController RuleInteraction { get; }

        private void Awake()
        {
            var eventBus = new GameEventBus();
            var knowledge = new KnowledgeState(eventBus);
            var progress = new GameProgress(eventBus);
            var world = new WorldState(initialRoomId, eventBus);
            var loop = new LoopManager(world, eventBus);
            var loopTimer = new LoopTimer(loopTimeLimitSeconds);
            loopEventScheduler = new LoopEventScheduler(world, eventBus, loopEventData);
            var loopEnd = new LoopEndController(loop, loopTimer, progress, eventBus);
            var roomAccess = new RoomAccessChecker(knowledge, progress);
            var rooms = new RoomManager(world, eventBus, roomData, roomAccess);
            ownerViewPresenter.Initialize(world.Owner, eventBus);
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
            var settings = SettingsState.Load();
            var dialogueTypewriter = new DialogueTypewriter(DialogueSpeed.Normal);
            dialogueTypewriter.SetSpeedMultiplier(
    settings.DialogueSpeed);
            var dialogueLog = new DialogueLog();
            var dialogueResolver = new DialogueResolver(dialogueData);
            var dialogueInteraction = new DialogueInteractionController(knowledgeSlot, dialogueResolver);
            DialoguePortraitRepository = new DialoguePortraitRepository(dialoguePortraitEntries);
            DialogueBackgroundRepository = new DialogueBackgroundRepository(dialogueBackgroundEntries);
            var dialogueManager = new DialogueManager(eventBus, dialoguePlayback, dialogueTypewriter, dialogueLog);
            loopDialogueController = new LoopDialogueController(progress, dialogueManager, loopTimer, eventBus);
            ownerDepartureController = new OwnerDepartureController(world.Owner, world.Customers, dialogueManager, loop, eventBus);
            var manualParser = new ManualParser();
            var manualPages = new ManualPageRepository(manualParser, manualSources);
            var manualImageRepository = new ManualImageRepository(manualImages);
            var browserManager = new BrowserManager(eventBus);
            var manualLinkInteraction = new ManualLinkInteractionController(browserManager, knowledgeSelection);
            var manualSearch = new ManualSearchManager(manualPages);
            var craftingStation = new CraftingStation(eventBus);
            var recipeMatcher = new RecipeMatcher(recipeData);
            var craftResultHandler = new CraftResultHandler(eventBus, recipeMatcher);
            var craftingStationInteraction = new CraftingStationInteractionController(knowledgeInteraction, dialogueInteraction, craftingStation);

            RuleParser ruleParser = new RuleParser();
            var ruleDataList = ruleParser.Parse(interactionRulesText.text);
            ConditionFactory conditionFactory = new ConditionFactory();
            EffectFactory effectFactory = new EffectFactory();
            RuleFactory ruleFactory = new RuleFactory(conditionFactory, effectFactory);
            var rules = ruleFactory.CreateAll(ruleDataList);
            RuleResolver ruleResolver = new RuleResolver(rules);
            RuleExecutor ruleExecutor = new RuleExecutor();
            RuleInteractionController ruleInteraction = new RuleInteractionController(ruleResolver, ruleExecutor);

            Context = new GameContext(
                eventBus,
                knowledge,
                progress,
                world,
                loop,
                loopTimer,
                loopEnd,
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
                craftingStation,
                craftingStationInteraction,
                ruleInteraction);

            ownerInteractionView.Initialize(Context);
            controlInteractionView.Initialize(Context);
            studyInteractionView.Initialize(Context);
            telephoneInteractionView.Initialize(Context);
            customerView.Initialize(eventBus);
            customerInteractionView.Initialize(Context);

            dialogueCanvasController.Initialize(Context.EventBus);

            KnowledgeSlotDialogueTrigger = new KnowledgeSlotDialogueTrigger(Context, knowledgeSlotDialogueTriggerData);
            KnowledgeSlotDialogueTrigger.Initialize();

            CraftingDialogueTrigger = new CraftingDialogueTrigger(Context, recipeMatcher, craftingDialogueTargetId);
            CraftingDialogueTrigger.Initialize();

            eventBus.Subscribe<LoopStartedEvent>(
    OnLoopStarted);

            eventBus.Subscribe<LoopEndedEvent>(
                OnLoopEnded);

            eventBus.Subscribe<LoopEventTriggeredEvent>(
    OnLoopEventTriggered);

            loopRestart = new LoopRestartController(loop, eventBus);

            Context.Loop.StartLoop();

            Context.Loop.StartLoop();

            loopDialogueController.StartBeginning();
        }

        private void Update()
        {

            Context.LoopTimer.Update(
                Time.deltaTime);

            loopEventScheduler.Update(Context.LoopTimer.ElapsedSeconds);

            Context.LoopEnd.Update();

            ownerDepartureController.Update(Time.deltaTime);

            loopRestart.Update();
        }

        private void OnLoopStarted(
    LoopStartedEvent eventData)
        {
            Context.LoopTimer.Start();
        }

        private void OnLoopEnded(
            LoopEndedEvent eventData)
        {
            Context.LoopTimer.Stop();
        }

        private void OnLoopEventTriggered(
    LoopEventTriggeredEvent eventData)
        {
            Debug.Log(
                $"Loop event triggered: {eventData.EventId}");
        }

        private void OnDestroy()
        {
            if (Context == null)
                return;

            Context.EventBus.Unsubscribe<LoopStartedEvent>(
                OnLoopStarted);

            Context.EventBus.Unsubscribe<LoopEndedEvent>(
                OnLoopEnded);

            Context.EventBus.Unsubscribe<LoopEventTriggeredEvent>(
                OnLoopEventTriggered);

            Context.LoopEnd.Dispose();
            loopEventScheduler.Dispose();
            loopDialogueController.Dispose();
            ownerDepartureController.Dispose();
            loopRestart.Dispose();
        }
    }
}
