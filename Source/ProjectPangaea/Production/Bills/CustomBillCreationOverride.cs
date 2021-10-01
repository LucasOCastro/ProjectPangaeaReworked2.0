using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(BillUtility), "MakeNewBill")]
    public static class CustomBillCreationOverride
    {
        public static bool Prefix(ref Bill __result, RecipeDef recipe)
        {
            //todo this will be moved to a custom window later
            RecipeExtension spliceExtension = recipe.GetModExtension<RecipeExtension>();
            if (spliceExtension != null)
            {
                __result = new PangaeaBill(recipe, spliceExtension.recipes.FirstOrFallback());
                return false;
            }
            return true;
        }
    }
}
