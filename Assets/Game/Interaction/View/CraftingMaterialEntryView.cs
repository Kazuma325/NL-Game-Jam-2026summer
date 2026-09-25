using TMPro;
using UnityEngine;

namespace ShopGame.Interaction.View
{
    public sealed class CraftingMaterialEntryView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text materialText;

        public void Initialize(string materialId)
        {
            if (string.IsNullOrWhiteSpace(materialId))
            {
                throw new System.ArgumentException(
                    "Material ID must not be null, empty, or whitespace.",
                    nameof(materialId));
            }

            materialText.text = materialId;
        }
    }
}