using System;
using System.Collections.Generic;
using ShopGame.Interaction.Data;

namespace ShopGame.Interaction.Runtime
{
    public sealed class RecipeMatcher
    {
        private readonly IReadOnlyList<RecipeData> recipes;

        public RecipeMatcher(
            IReadOnlyList<RecipeData> recipes)
        {
            this.recipes =
                recipes
                ?? throw new ArgumentNullException(
                    nameof(recipes));
        }

        public bool TryMatch(
            IReadOnlyCollection<string> materialIds,
            out string itemId)
        {
            if (materialIds == null)
            {
                throw new ArgumentNullException(
                    nameof(materialIds));
            }

            foreach (RecipeData recipe in recipes)
            {
                if (recipe == null)
                {
                    continue;
                }

                if (IsSameMaterialSet(
                        materialIds,
                        recipe.MaterialIds))
                {
                    itemId = recipe.ItemId;
                    return true;
                }
            }

            itemId = null;
            return false;
        }

        private static bool IsSameMaterialSet(
            IReadOnlyCollection<string> materialIds,
            IReadOnlyCollection<string> recipeMaterialIds)
        {
            if (materialIds.Count != recipeMaterialIds.Count)
            {
                return false;
            }

            var recipeMaterialSet =
                new HashSet<string>(recipeMaterialIds);

            return recipeMaterialSet.SetEquals(materialIds);
        }
    }
}