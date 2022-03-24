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
                recipe.label = baseRecipe.label.Formatted(entry.ThingDef.label);

                /*var icon = entry.ThingDef.GetIcon();
                var graphic = entry.ThingDef.graphic;
                Log.Message(icon.NullOrBad().ToStringSafe());
                recipe.definingIcon = icon;
                recipe.definingGraphic = graphic;
                if (recipe.canBeReversed)
                {
                    recipe.Reversed.definingIcon = icon;
                    recipe.Reversed.definingGraphic = graphic;
                }
                recipe.DefiningPortion = new PortionData(entry.ThingDef, 1);
                Log.Message("ga"+recipe.DefiningPortion.Icon.NullOrBad().ToStringSafe());*/
                recipe.definingPortion = new PortionData(entry.ThingDef, 0);
                if (recipe.canBeReversed)
                {
                    recipe.Reversed.definingPortion = recipe.definingPortion;
                }

                //recipe.definingIcon = recipe.DefiningPortion.Icon;
                //recipe.definingGraphic = recipe.DefiningPortion.Graphic;
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