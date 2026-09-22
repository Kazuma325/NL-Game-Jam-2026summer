using System;
using UnityEngine;

namespace ShopGame.Adventure.Data
{
    [Serializable]
    public sealed class RoomAccessRequirementData
    {
        [SerializeField]
        private RoomAccessRequirementType type;
        
        [SerializeField]
        private string requirementId;

        public RoomAccessRequirementType Type => type;
        public string RequirementId => requirementId;
    }
}
