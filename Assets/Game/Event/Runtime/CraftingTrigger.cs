using System;

namespace ShopGame.Event.Runtime
{
    public sealed class CraftingTrigger : ITrigger
    {
        public string RecipeId { get; }

        public CraftingTrigger(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId))
            {
                throw new ArgumentException(
                    "Recipe ID must not be null, empty, or whitespace.",
                    nameof(recipeId));
            }

            RecipeId = recipeId;
        }
    }
}