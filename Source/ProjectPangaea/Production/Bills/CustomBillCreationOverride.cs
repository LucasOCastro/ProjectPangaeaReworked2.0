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
            if (TryGetCounterAndEntries(recipe, out List<PangaeaThingEntry> entries, out PangaeaBillCounter counter))
            {
                __result = new PangaeaResourceBill(recipe, entries, counter);
                return false;
            }
            return true;
        }

        private static bool TryGetCounterAndEntries(RecipeDef recipe, out List<PangaeaThingEntry> entries, out PangaeaBillCounter counter)
        {
            counter = null;
            entries = null;

            var resourceExtension = recipe.GetModExtension<Pangaea_ResourceRecipeExtension>();
            if (resourceExtension != null)
            {
                counter = resourceExtension.GetBillCounter(recipe);
                entries = resourceExtension.GetListerEntries(recipe);
            }

            return entries != null && counter != null;
        }
    }
}
