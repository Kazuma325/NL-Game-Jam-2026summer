using System;

namespace ShopGame.Dialogue.Data
{
    [Serializable]
    public sealed class DialogueCraftingCondition
    {
        public DialogueCraftingConditionType Type;
        public string ItemId;
    }
}