using System;
using ShopGame.Adventure.Data;
using ShopGame.State.Knowledge;
using ShopGame.State.Progress;
using UnityEngine;

namespace ShopGame.Adventure.Runtime
{
    public sealed class RoomAccessChecker
    {
        private readonly KnowledgeState knowledgeState;
        private readonly GameProgress gameProgress;

        public RoomAccessChecker(KnowledgeState knowledgeState, GameProgress gameProgress)
        {
            this.knowledgeState = knowledgeState ?? throw new ArgumentNullException(nameof(knowledgeState));
            this.gameProgress = gameProgress ?? throw new ArgumentNullException(nameof(gameProgress));
        }

        public bool CanAccess(RoomConnectionData connection)
        {
            if(connection == null) throw new ArgumentNullException(nameof (connection));

            RoomAccessRequirementData requirement = connection.AccessRequirement;

            if(requirement == null) return true;

            switch (requirement.Type)
            {
                case RoomAccessRequirementType.None:
                    return true;

                case RoomAccessRequirementType.Knowledge:
                    return knowledgeState.HasKnowledge(requirement.RequirementId);

                case RoomAccessRequirementType.Progress:
                    return gameProgress.HasProgress(requirement.RequirementId);

                default:
                    throw new ArgumentOutOfRangeException(nameof(requirement), requirement.Type, "Unsupported room access requirement type.");
            }
        }
    }
}
