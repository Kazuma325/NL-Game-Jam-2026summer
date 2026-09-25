using System;
using UnityEngine;

namespace ShopGame.Adventure.Data
{
    [Serializable]
    public sealed class RoomConnectionData
    {
        [SerializeField]
        private string connectionId;

        [SerializeField]
        private string targetRoomId;

        [SerializeField]
        private RoomConnectionDirection direction;

        [SerializeField]
        private RoomAccessRequirementData accessRequirement;

        [SerializeField]
        private string requiredKnowledgeId;

        public string ConnectionId => connectionId;
        public string TargetRoomId => targetRoomId;
        public RoomConnectionDirection Direction => direction;
        public RoomAccessRequirementData AccessRequirement => accessRequirement;
        public string RequiredKnowledgeId => requiredKnowledgeId;
    }
}
