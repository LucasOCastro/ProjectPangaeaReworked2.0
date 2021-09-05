using HarmonyLib;
using RimWorld;
using Verse;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(Bill), "IsFixedOrAllowedIngredient", typeof(Thing))]
    public static class CustomBillAllowanceOverride
    {
        public static void Postfix(ref bool __result, Bill __instance, Thing thing)
        {
            if (!__result)
            {
                return;
            }

            if (!(__instance is PangaeaResourceBill pangaeaBill))
            {
                return;
            }

            if (PangaeaDatabase.TryGetEntryFromThing(thing, out PangaeaThingEntry entry, out bool shouldHaveEntry))
            {
                __result = pangaeaBill.AllowedEntries.Contains(entry);
            }
            else if (shouldHaveEntry)
            {
                __result = shouldHaveEntry;
            }
        }
    }
}
