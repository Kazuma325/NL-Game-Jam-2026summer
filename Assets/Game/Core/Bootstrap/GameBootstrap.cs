using ShopGame.Core.Context;
using GameEventBus = ShopGame.Core.EventBus.EventBus;
using ShopGame.Loop;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using ShopGame.State.World;
using UnityEngine;

namespace ShopGame.Core.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private string initialRoomId = "Shop";

        public GameContext Context { get; private set; }

        private void Awake()
        {
            var eventBus = new GameEventBus();
            var knowledge = new KnowledgeState(eventBus);
            var progress = new GameProgress(eventBus);
            var world = new WorldState(initialRoomId);
            var loop = new LoopManager(world, eventBus);

            Context = new GameContext(eventBus, knowledge, progress, world, loop);
        }
    }
}
