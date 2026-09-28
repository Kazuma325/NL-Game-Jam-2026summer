using ShopGame.Core.EventBus;
using ShopGame.Core.EventBus.ShopGame.Core.EventBus;
using System;
using UnityEngine;

namespace ShopGame.Event.View
{
    public sealed class CustomerView : MonoBehaviour
    {
        [SerializeField]
        private GameObject customerObject;

        private EventBus eventBus;

        public void Initialize(EventBus eventBus)
        {
            if (eventBus == null)
            {
                throw new ArgumentNullException(
                    nameof(eventBus));
            }

            if (customerObject == null)
            {
                throw new InvalidOperationException(
                    "Customer Object is not assigned.");
            }

            this.eventBus = eventBus;

            eventBus.Subscribe<CustomerStateChangedEvent>(
                OnCustomerStateChanged);

            customerObject.SetActive(false);
        }

        private void OnCustomerStateChanged(
            CustomerStateChangedEvent eventData)
        {
            customerObject.SetActive(
                eventData.IsPresent);
        }

        private void OnDestroy()
        {
            if (eventBus == null)
            {
                return;
            }

            eventBus.Unsubscribe<CustomerStateChangedEvent>(
                OnCustomerStateChanged);
        }
    }
}