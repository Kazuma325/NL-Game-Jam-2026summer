using UnityEngine;

namespace ShopGame.Dialogue.Data
{
    [CreateAssetMenu(
        fileName = "KnowledgeDialogueTriggerData",
        menuName = "ShopGame/Dialogue/Knowledge Slot Dialogue Trigger Data")]
    public sealed class KnowledgeSlotDialogueTriggerData : ScriptableObject
    {
        [SerializeField] private string knowledgeId;
        [SerializeField] private string targetId;

        public string KnowledgeId => knowledgeId;
        public string TargetId => targetId;
    }
}