using ShopGame.Core.EventBus;
using ShopGame.Core.EventBus.ShopGame.Core.EventBus;
using System;

namespace ShopGame.State.World
{
    public sealed class CustomerState
    {
        private readonly EventBus eventBus;

        public bool IsPresent { get; private set; }

        public CustomerState(EventBus eventBus)
        {
            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(nameof(eventBus));

            IsPresent = false;
        }

        public void Appear()
        {
            if (IsPresent)
            {
                return;
            }

            IsPresent = true;

            eventBus.Publish(
                new CustomerStateChangedEvent(IsPresent));
        }

        public void Leave()
        {
            if (!IsPresent)
            {
                return;
            }

            IsPresent = false;

            eventBus.Publish(
                new CustomerStateChangedEvent(IsPresent));
        }

        public void Reset()
        {
            IsPresent = false;

            eventBus.Publish(
                new CustomerStateChangedEvent(IsPresent));
        }
    }
}