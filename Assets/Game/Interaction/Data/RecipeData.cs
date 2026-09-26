using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Interaction.Data
{
    [CreateAssetMenu(
        fileName = "RecipeData",
        menuName = "ShopGame/Interaction/Recipe Data")]
    public sealed class RecipeData : ScriptableObject
    {
        [SerializeField]
        private string itemId;

        [SerializeField]
        private string[] materialIds = Array.Empty<string>();

        public string ItemId => itemId;

        public IReadOnlyList<string> MaterialIds =>
            materialIds;
    }
}