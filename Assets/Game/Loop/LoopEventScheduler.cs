using System;
using System.Collections.Generic;
using ShopGame.Core.EventBus;
using ShopGame.Loop.Data;
using ShopGame.State.World;

namespace ShopGame.Loop
{
    public sealed class LoopEventScheduler
    {
        private readonly WorldState worldState;
        private readonly EventBus eventBus;
        private readonly IReadOnlyList<LoopEventData> eventDataList;

        private readonly HashSet<string> triggeredEventIds = new();

        public LoopEventScheduler(
            WorldState worldState,
            EventBus eventBus,
            IReadOnlyList<LoopEventData> eventDataList)
        {
            this.worldState =
                worldState
                ?? throw new ArgumentNullException(
                    nameof(worldState));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            this.eventDataList =
                eventDataList
                ?? throw new ArgumentNullException(
                    nameof(eventDataList));

            eventBus.Subscribe<LoopStartedEvent>(
                OnLoopStarted);
        }

        public void Update(float elapsedSeconds)
        {
            foreach (LoopEventData eventData in eventDataList)
            {
                if (eventData == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(eventData.EventId))
                {
                    continue;
                }

                if (triggeredEventIds.Contains(eventData.EventId))
                {
                    continue;
                }

                if (elapsedSeconds < eventData.TriggerTimeSeconds)
                {
                    continue;
                }

                TriggerEvent(eventData);
            }
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<LoopStartedEvent>(
                OnLoopStarted);
        }

        private void OnLoopStarted(
            LoopStartedEvent eventData)
        {
            triggeredEventIds.Clear();
        }

        private void TriggerEvent(
            LoopEventData eventData)
        {
            triggeredEventIds.Add(eventData.EventId);

            worldState.Events.Activate(
                eventData.EventId);

            eventBus.Publish(
                new LoopEventTriggeredEvent(
                    eventData.EventId));
        }
    }
}