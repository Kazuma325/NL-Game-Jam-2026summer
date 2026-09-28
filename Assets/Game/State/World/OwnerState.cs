using System;
using ShopGame.Core.EventBus;

namespace ShopGame.State.World
{
    public sealed class OwnerState
    {
        private readonly EventBus eventBus;

        public bool IsPresent { get; private set; }

        public OwnerState(EventBus eventBus)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            IsPresent = true;
        }

        public void Depart()
        {
            if (!IsPresent)
            {
                return;
            }

            IsPresent = false;

            eventBus.Publish(
                new OwnerStateChangedEvent(IsPresent));
        }

        public void Return()
        {
            if (IsPresent)
            {
                return;
            }

            IsPresent = true;

            eventBus.Publish(
                new OwnerStateChangedEvent(IsPresent));
        }

        public void Reset()
        {
            IsPresent = true;

            eventBus.Publish(
                new OwnerStateChangedEvent(IsPresent));
        }
    }
}