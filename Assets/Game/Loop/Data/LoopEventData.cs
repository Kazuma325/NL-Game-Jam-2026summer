using System;
using UnityEngine;

namespace ShopGame.Loop.Data
{
    [CreateAssetMenu(
        fileName = "LoopEventData",
        menuName = "ShopGame/Loop/Loop Event Data")]
    public sealed class LoopEventData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string eventId;

        [Header("Timing")]
        [SerializeField]
        private float triggerTimeSeconds;

        public string EventId => eventId;

        public float TriggerTimeSeconds => triggerTimeSeconds;
    }
}