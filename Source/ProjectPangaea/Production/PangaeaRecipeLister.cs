using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    public static class PangaeaRecipeLister
    {
        private static List<RecipeDef> recipes = new List<RecipeDef>();
        public static IEnumerable<RecipeDef> AllPangaeaRecipes => recipes;

        private static Dictionary<RecipeDef, RecipeExtension> recipeExtensions = new Dictionary<RecipeDef, RecipeExtension>();
        public static IEnumerable<RecipeExtension> AllRecipeExtensions => recipeExtensions.Values;

        public static void Init()
        {
            foreach (RecipeDef recipe in DefDatabase<RecipeDef>.AllDefs)
            {
                RecipeExtension extension = recipe.GetModExtension<RecipeExtension>();
                if (extension == null)
                {
                    continue;
                }

                recipes.Add(recipe);
                recipeExtensions.Add(recipe, extension);
            }
        }
    }
}
