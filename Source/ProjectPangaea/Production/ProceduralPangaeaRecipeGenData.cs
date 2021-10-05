using System.Collections.Generic;
using Verse;

namespace ProjectPangaea.Production
{
    [System.Serializable]
    public class ProceduralPangaeaRecipeGenData
    {
        private PangaeaRecipeSettings baseRecipe = null;
        private List<PortionDataGenerator> ingredients = new List<PortionDataGenerator>();
        private List<PortionDataGenerator> results = new List<PortionDataGenerator>();

        public PangaeaRecipeSettings GenRecipe(PangaeaThingEntry entry)
        {
            var recipe = new PangaeaRecipeSettings();
            if (baseRecipe != null)
            {
                recipe.ingredients = new List<PortionData>(baseRecipe.ingredients);
                recipe.results = new List<PortionData>(baseRecipe.results);
                recipe.canBeReversed = baseRecipe.canBeReversed;
                recipe.stackCountProcessors = new List<StackCountProcessor>(baseRecipe.stackCountProcessors);
            }

            bool genFromList(List<PortionDataGenerator> genFrom, List<PortionData> target)
            {
                foreach (var item in genFrom)
                {
                    var result = item.GenFor(entry);
                    if (result == null)
                        return false;
                    target.Add(result);
                }
                return true;
            }

            if (!genFromList(ingredients, recipe.ingredients) || !genFromList(results, recipe.results))
            {
                return null;
            }
            return recipe;
        }
    }
}