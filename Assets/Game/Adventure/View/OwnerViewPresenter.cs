using System;
using ShopGame.Core.EventBus;
using ShopGame.State.World;
using UnityEngine;

namespace ShopGame.Adventure.View
{
    public sealed class OwnerViewPresenter : MonoBehaviour
    {
        [SerializeField]
        private OwnerView ownerView;

        private OwnerState ownerState;
        private EventBus eventBus;

        public void Initialize(
    OwnerState ownerState,
    EventBus eventBus)
        {
            this.ownerState =
                ownerState
                ?? throw new ArgumentNullException(
                    nameof(ownerState));

            this.eventBus =
                eventBus
                ?? throw new ArgumentNullException(
                    nameof(eventBus));

            if (ownerView == null)
            {
                throw new InvalidOperationException(
                    "Owner View is not assigned.");
            }

            eventBus.Subscribe<OwnerStateChangedEvent>(
                OnOwnerStateChanged);

            Refresh();
        }

        private void Refresh()
        {
            ownerView.SetPresent(
                ownerState.IsPresent);
        }

        private void OnOwnerStateChanged(
    OwnerStateChangedEvent eventData)
        {
            ownerView.SetPresent(eventData.IsPresent);
        }

        private void OnDestroy()
        {
            if (eventBus == null)
            {
                return;
            }

            eventBus.Unsubscribe<OwnerStateChangedEvent>(
                OnOwnerStateChanged);
        }
    }
}