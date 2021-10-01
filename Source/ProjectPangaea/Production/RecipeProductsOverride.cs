using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using ProjectPangaea.Production.Splicing;


namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(GenRecipe), "MakeRecipeProducts")]
    public class RecipeProductsOverride
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> originalResult, RecipeDef recipeDef, List<Thing> ingredients)
        {
            foreach (Thing result in originalResult)
            {
                yield return result;
            }

            var recipeExtension = recipeDef.GetModExtension<RecipeExtension>();
            if (recipeExtension != null)
            {
                foreach (var result in CurrentBillGetter.CurrentBill.MakeResults())
                    yield return result;
                yield break;
            }
        }
    }
}
