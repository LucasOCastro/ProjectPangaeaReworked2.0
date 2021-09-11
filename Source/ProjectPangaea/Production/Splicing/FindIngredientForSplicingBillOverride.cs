using Verse;
using RimWorld;
using HarmonyLib;
using System.Collections.Generic;

namespace ProjectPangaea.Production.Splicing
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet_NoMix")]
    public static class FindIngredientForSplicingBillOverride
    {
        private static bool Prefix(ref bool __result, List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
        {
            if (bill is DNASplicingBill)
            {
                __result = ResourceSelectionForBillReversePatch(availableThings, bill, chosen, rootCell, alreadySorted);
                return false;
            }
            return true;
        }

        [HarmonyReversePatch(HarmonyReversePatchType.Original)]
        private static bool ResourceSelectionForBillReversePatch(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return VanillaFindIngredientsFix.FixFindIngredientBugs(instructions, out _, out _);
            }
            _ = Transpiler(null);
            return default;
        }
    }
}
