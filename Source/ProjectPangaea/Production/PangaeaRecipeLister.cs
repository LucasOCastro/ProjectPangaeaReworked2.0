using System.Collections.Generic;
using System.Linq;
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
            ProjectPangaeaMod.AssertDatabaseInit(nameof(PangaeaRecipeLister));

            foreach (RecipeDef recipe in DefDatabase<RecipeDef>.AllDefs)
            {
                RecipeExtension extension = recipe.GetModExtension<RecipeExtension>();
                if (extension == null)
                {
                    continue;
                }

                foreach (var procedural in extension.proceduralRecipeDefs)
                {
                    foreach (var entry in PangaeaDatabase.AllEntries)
                    {
                        var generated = procedural.GenRecipe(entry);
                        if (generated != null)
                        {
                            extension.recipes.Add(generated);
                        }
                    }
                }

                recipes.Add(recipe);
                recipeExtensions.Add(recipe, extension);

                AddDummyIngredientIfNeeded(recipe, extension);
            }
        }

        //If there's no ingredient in the normal recipeDef, itll not even try looking for them
        //Could also be fixed with a harmony patch somewhere
        private static void AddDummyIngredientIfNeeded(RecipeDef recipe, RecipeExtension extension)
        {
            if (!recipe.ingredients.NullOrEmpty())
            {
                return;
            }

            if (recipe.fixedIngredientFilter.AllowedDefCount == 0)
            {
                recipe.fixedIngredientFilter = extension.GenerateGeneralThingFilter();
            }

            if (recipe.ingredients.NullOrEmpty())
            {
                IngredientCount dummyIngredient = new IngredientCount()
                {
                    filter = recipe.fixedIngredientFilter,
                };
                recipe.ingredients.Add(dummyIngredient);
            }
        }
    }
}
