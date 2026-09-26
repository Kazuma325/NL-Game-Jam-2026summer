using System;

namespace ShopGame.Dialogue.Data
{
    [Serializable]
    public sealed class DialogueKnowledgeCondition
    {
        public DialogueKnowledgeConditionType Type;

        public string KnowledgeId;
    }
}