using Verse;
using HarmonyLib;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    public class SplicingProductsOverride
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> originalResult, RecipeDef recipeDef, List<Thing> ingredients)
        {
            foreach (Thing result in originalResult)
            {
                yield return result;
            }

            var splicingExtension = recipeDef.GetModExtension<Pangaea_DNASplicingRecipeExtension>();
            if (splicingExtension is null)
            {
                yield break;
            }

            //TODO make this shit better this aint what i want
            foreach (Thing ingredient in ingredients)
            {
                if (ingredient is DNAThing dnaThing)
                {
                    foreach (Thing spliceResult in DNASplicingWorker.GetSpliceResults(dnaThing))
                    {
                        yield return spliceResult;
                    }
                }
            }
        }
    }
}
