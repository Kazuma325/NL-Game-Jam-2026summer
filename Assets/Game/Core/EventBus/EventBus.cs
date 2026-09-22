using System;
using System.Collections.Generic;

namespace ShopGame.Core.EventBus
{
    /// <summary>
    /// Publishes in-process notifications to subscribers registered for each event type.
    /// </summary>
    public sealed class EventBus
    {
        private readonly Dictionary<Type, Delegate> handlersByEventType = new();

        public void Subscribe<T>(Action<T> handler) where T : struct
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Type eventType = typeof(T);
            handlersByEventType.TryGetValue(eventType, out Delegate existingHandlers);
            handlersByEventType[eventType] = Delegate.Combine(existingHandlers, handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Type eventType = typeof(T);
            if (!handlersByEventType.TryGetValue(eventType, out Delegate existingHandlers))
            {
                return;
            }

            Delegate remainingHandlers = Delegate.Remove(existingHandlers, handler);
            if (remainingHandlers == null)
            {
                handlersByEventType.Remove(eventType);
                return;
            }

            handlersByEventType[eventType] = remainingHandlers;
        }

        public void Publish<T>(T eventData) where T : struct
        {
            if (handlersByEventType.TryGetValue(typeof(T), out Delegate handlers))
            {
                ((Action<T>)handlers).Invoke(eventData);
            }
        }
    }
}
