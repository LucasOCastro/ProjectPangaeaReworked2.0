using RimWorld;
using Verse;
using HarmonyLib;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(BillUtility), "MakeNewBill")]
    public static class CustomBillCreationOverride
    {
        public static bool Prefix(ref Bill __result, RecipeDef recipe)
        {
            //todo this will be moved to a custom window later
            RecipeExtension recipeExtension = recipe.GetModExtension<RecipeExtension>();
            if (recipeExtension != null)
            {
                PangaeaRecipeSettings recipeSet = recipeExtension.recipes.FirstOrFallback();
                __result = new PangaeaBill(recipe, recipeSet);
                return false;
            }
            return true;
        }
    }
}
