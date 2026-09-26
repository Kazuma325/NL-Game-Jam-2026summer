using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Dialogue.Data
{
    [CreateAssetMenu(
        fileName = "DialogueData",
        menuName = "ShopGame/Dialogue/Dialogue Data")]
    public sealed class DialogueData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string dialogueId;

        [SerializeField]
        private string targetId;

        [SerializeField]
        private int priority;

        [Header("Knowledge")]
        [SerializeField]
        private DialogueKnowledgeCondition knowledgeCondition =
            new DialogueKnowledgeCondition();

        [Header("Progress")]
        [SerializeField]
        private string[] requiredProgressIds = Array.Empty<string>();

        [Header("Loop")]
        [SerializeField]
        private int minimumLoopCount = 0;

        [SerializeField]
        private int maximumLoopCount = -1;

        [Header("World")]
        [SerializeField]
        private string requiredRoomId;

        [SerializeField]
        private string requiredDeviceStatus;

        [SerializeField]
        private string[] requiredActiveEventIds =
            Array.Empty<string>();

        [Header("Result")]
        [SerializeField]
        private bool consumeKnowledge;

        public string DialogueId => dialogueId;

        public string TargetId => targetId;

        public int Priority => priority;

        public DialogueKnowledgeCondition KnowledgeCondition =>
            knowledgeCondition;

        public IReadOnlyList<string> RequiredProgressIds =>
            requiredProgressIds;

        public int MinimumLoopCount => minimumLoopCount;

        public int MaximumLoopCount => maximumLoopCount;

        public string RequiredRoomId => requiredRoomId;

        public string RequiredDeviceStatus =>
            requiredDeviceStatus;

        public IReadOnlyList<string> RequiredActiveEventIds =>
            requiredActiveEventIds;

        public bool ConsumeKnowledge => consumeKnowledge;
    }
}