using Verse;
using HarmonyLib;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
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

            /*foreach (Thing result in DNASplicingWorker.GenRecipeResults(recipeDef, ingredients))
            {
                yield return result;
            }*/
            //TODO currently guessing, but i want to pass directly from bill. look into 'internal void <FinishRecipeAndStartStoringProduct>b__0()'
            if (DNASplicingWorker.TryGuessDefFromIngredients(ingredients, out DNASplicingDef splicingDef, out bool divideDNA))
            {
                if (divideDNA)
                {
                    foreach (var result in splicingDef.MakePortionThings())
                        yield return result;
                }
                else yield return splicingDef.MakeResultThing();
            }
        }
    }
}
