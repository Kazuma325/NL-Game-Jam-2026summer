using System;
using ShopGame.Adventure.Runtime;
using GameEventBus = ShopGame.Core.EventBus.EventBus;
using ShopGame.Loop;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using ShopGame.State.World;

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

        public GameContext(
            GameEventBus eventBus,
            KnowledgeState knowledge,
            GameProgress progress,
            WorldState world,
            LoopManager loop,
            RoomManager rooms,
            RoomAccessChecker roomAccess,
            RoomNavigationController navigation)
        {
            EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            Knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
            Progress = progress ?? throw new ArgumentNullException(nameof(progress));
            World = world ?? throw new ArgumentNullException(nameof(world));
            Loop = loop ?? throw new ArgumentNullException(nameof(loop));
            Rooms = rooms ?? throw new ArgumentNullException(nameof(rooms));
            RoomAccess = roomAccess ?? throw new ArgumentNullException(nameof(roomAccess));
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

        }
    }
}
