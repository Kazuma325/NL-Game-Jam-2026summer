# Shop Adventure Game

## Project Overview

This is a Unity 2D adventure game built around a repeating time-loop structure.

The player operates a shop and investigates events through:

* Exploration of the shop
* Dialogue with customers
* An in-game PC
* A Wikipedia-like manual/browser
* Knowledge discovered across loops
* Events and conditions
* Game-over and loop restart mechanics

The project prioritizes a clean, extensible architecture over rapid prototyping.

---

## Technology

* Unity 6
* C#
* Unity UI
* ScriptableObjects for authored/static data
* Plain C# classes for runtime state and domain logic

---

## Core Architecture

Use the following conceptual separation:

Data
→ authored/static game data, primarily ScriptableObjects

Runtime
→ runtime state and game/domain logic, primarily plain C# classes

View
→ Unity MonoBehaviours responsible for presentation and user input forwarding

Do not mix these responsibilities without a clear reason.

---

## Architecture Principles

### 1. Keep View classes thin

MonoBehaviour/View classes should primarily:

* Receive Unity input
* Display state
* Forward user actions to the appropriate runtime system

Do not put core game logic in View classes.

Examples of forbidden patterns:

* BombView directly restarting the loop
* DialogueView directly modifying KnowledgeState
* TabView directly querying and modifying ManualDatabase
* View classes directly changing WorldState

---

### 2. Separate Data from Runtime State

ScriptableObjects represent authored/static data.

Runtime state must not be stored in ScriptableObjects.

Examples:

* ManualPageData → ScriptableObject
* DialogueData → ScriptableObject
* GameEventData → ScriptableObject
* RoomData → ScriptableObject

Runtime examples:

* KnowledgeState
* GameProgress
* WorldState
* BrowserTab
* PageHistory
* DialogueRuntime
* GameEventRuntime

---

### 3. Keep responsibilities narrow

Each class should have one clear primary responsibility.

Do not introduce large "manager of everything" classes.

Avoid creating classes such as:

* GameManager
* GameSystemManager
* UniversalManager

unless a concrete architectural need has been discussed and approved.

---

### 4. EventBus is for notifications, not commands

Use EventBus for system-to-system notifications.

Examples:

* KnowledgeAddedEvent
* ProgressCompletedEvent
* RoomChangedEvent
* LoopStartedEvent
* LoopEndedEvent
* GameOverEvent

Prefer direct method calls for explicit commands.

Examples:

* BrowserManager.OpenPage()
* RoomManager.ChangeRoom()
* DialogueManager.StartDialogue()
* EventManager.StartEvent()

Do not use EventBus merely to avoid a normal method call.

---

### 5. Do not create unnecessary dependencies

Important dependency rules:

* Manual must not depend on Browser.
* Manual data must not depend on BrowserManager.
* Browser may depend on ManualDatabase.
* TabView must not directly query ManualDatabase.
* State classes must not directly manipulate Views.
* Condition classes must not mutate game state.
* Action classes may mutate state through GameContext.
* Interaction/View classes must not bypass the appropriate Manager.

Preserve dependency direction when adding new features.

---

### 6. Stable IDs

Use stable internal IDs for game entities and authored content.

Examples:

* PageId
* DialogueId
* EventId
* RoomId
* KnowledgeId
* ProgressId

Display names and titles must not be used as internal identifiers.

---

## State Architecture

The game has three distinct categories of state.

### KnowledgeState

Represents what the player knows.

Knowledge persists across loops.

Examples:

* bomb_exists
* bomb_disarm_method
* secret_room_exists
* manager_password

---

### GameProgress

Represents persistent story/game progress.

Progress persists across loops.

Examples:

* bomb_event_cleared
* secret_room_opened
* manager_secret_discovered

---

### WorldState

Represents the current state of the current loop.

WorldState resets when a loop restarts.

Examples:

* CurrentRoomId
* BombState
* CustomerState
* EventState

Do not store persistent knowledge or progress inside WorldState.

---

## Loop Architecture

GameStateMachine and LoopManager have different responsibilities.

GameStateMachine:

* Overall game state
* Title
* Playing
* GameOver
* LoopRestart
* Ending

LoopManager:

* Loop lifecycle
* StartLoop()
* EndLoop()
* RestartLoop()

Restarting a loop resets WorldState but preserves KnowledgeState and GameProgress.

---

## GameContext

GameContext is a runtime dependency container/execution environment.

It may provide references to runtime systems such as:

* KnowledgeState
* GameProgress
* WorldState
* EventBus
* GameStateMachine
* LoopManager
* DialogueManager
* BrowserManager
* RoomManager
* EventManager

GameContext itself should not contain game logic.

Runtime objects should be constructed by GameBootstrap rather than independently creating duplicate state objects.

---

## GameBootstrap

GameBootstrap is the Composition Root.

It is responsible for constructing and connecting runtime systems.

Do not allow individual managers to independently construct shared runtime state when that state should be shared.

---

## Interaction Architecture

The adventure portion contains clickable/interactable objects such as:

* PC
* Doors
* Customers
* Bombs
* Safes
* Other shop objects

Interaction input and game consequences should remain separate.

Conceptually:

View/Input
→ Interaction
→ appropriate Manager/Event
→ Runtime State

Examples:

PC interaction
→ BrowserManager

Customer interaction
→ DialogueManager

Bomb interaction
→ EventManager

Door interaction
→ RoomManager

Do not make an interactable View directly perform unrelated game-system operations.

---

## Manual Architecture

Manual data represents the contents of the in-game manual.

ManualPageData is authored data.

ManualDatabase provides access to manual pages.

ManualSearchService handles global manual search.

PageContentSearchService handles searching within the currently displayed page.

Manual pages must not directly control BrowserManager.

Link data stores a TargetPageId.

The decision between opening in the current tab or a new tab belongs to the Browser system.

---

## Browser Architecture

BrowserManager owns browser runtime state.

BrowserTab owns tab state.

PageHistory owns page navigation history.

BrowserView and TabView display browser state and forward user input.

Important rules:

* Search tab is always the leftmost tab.
* Search tab cannot be closed.
* Search tab cannot be duplicated.
* Normal link click opens in the current tab.
* Ctrl+click opens in a new tab.
* Each tab has independent page history.
* Opening a new page after going back truncates forward history.

Do not move browser logic into Manual classes.

---

## Event Architecture

GameEventData represents authored event data.

GameEventRuntime represents runtime event state.

EventManager controls event execution.

Conditions read state but do not mutate it.

Actions perform state changes.

GameContext provides the execution environment.

Do not create a universal event system capable of arbitrary behavior unless a concrete requirement justifies it.

Start with simple AND-based condition lists and extend only when necessary.

---

## Change Policy

Before changing architecture:

1. Inspect the existing implementation.
2. Identify the affected classes and dependencies.
3. Check whether the change conflicts with the established architecture.
4. If an architectural change is required, explain the proposed change before implementing it.

Do not silently redesign existing architecture.

Do not introduce new abstractions solely for theoretical future flexibility.

Prefer the simplest implementation that satisfies the established architecture.

---

## Existing UML Is Authoritative

The project's UML and architecture documentation define the intended class responsibilities and dependencies.

Implementation should follow the UML.

If implementation reveals a genuine problem in the UML:

1. Stop before making a large architectural change.
2. Explain the problem.
3. Propose the smallest appropriate design change.
4. Update the design documentation/UML after the change is approved.
5. Then implement the change.

---

## Implementation Workflow

For non-trivial tasks:

1. Inspect relevant existing files.
2. State the implementation plan.
3. Implement only the requested scope.
4. Run appropriate compilation/tests/checks.
5. Review the resulting diff.
6. Report:

   * Files changed
   * What was implemented
   * Tests/checks performed
   * Any remaining issues
   * Any architectural concerns

Do not expand the task scope without justification.

---

## Testing and Verification

After implementing a feature:

* Check for C# compilation errors.
* Run relevant automated tests if available.
* Verify that the implementation matches the requested API and responsibilities.
* Review the diff for unintended changes.
* Do not claim a feature works if it has not been verified.

Unity Editor-only behavior should be clearly identified when it cannot be verified automatically.

---

## Git

Keep changes focused.

Prefer small, coherent commits.

Do not modify unrelated files.

Do not rewrite history or perform destructive Git operations unless explicitly requested.

Before committing, inspect the diff and confirm that only intended files changed.

---

## Code Style

Prefer:

* Clear names
* Explicit responsibilities
* Small methods
* Private fields with controlled access
* Stable IDs
* Early returns where they improve readability

Avoid:

* God classes
* Excessive abstraction
* Hidden global state
* Static mutable game state
* Unnecessary singletons
* Deep inheritance hierarchies without a concrete need

---

## Important Rule

If a requested implementation conflicts with these architectural rules, do not silently work around the conflict.

Explain the conflict and propose a solution first.
