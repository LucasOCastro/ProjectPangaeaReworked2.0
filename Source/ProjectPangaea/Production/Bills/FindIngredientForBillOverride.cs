using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System;

namespace ProjectPangaea.Production
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet_NoMix")]
    public static class FindIngredientForBillOverride
    {
        [HarmonyPrefix]
        private static bool ProcessAvailableThingsPrefix(ref bool __result, List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
        {
            Pangaea_ResourceRecipeExtension resourceExtension = bill.recipe?.GetModExtension<Pangaea_ResourceRecipeExtension>();
            if (resourceExtension == null || resourceExtension.allowMixingResources)
            {
                return true;
            }

            PangaeaBillIngFinder.Clear();
            for (int i = 0; i < availableThings.Count; i++)
            {
                Thing thing = availableThings[i];

                if (thing is PangaeaThing pangaeaThing)
                {
                    PangaeaBillIngFinder.RegisterThing(pangaeaThing);
                }
            }
            __result =  ResourceSelectionForBillReversePatch(availableThings, bill, chosen, rootCell, alreadySorted);
            return false;
        }

        private static Type defCountListType = typeof(WorkGiver_DoBill).GetNestedType("DefCountList", BindingFlags.NonPublic);
        private static MethodInfo getDefMethodInfo = AccessTools.Method(defCountListType, "GetDef", new Type[] { typeof(int) });
        private static MethodInfo shouldSkipResourceMethodInfo = AccessTools.Method(typeof(PangaeaBillIngFinder), nameof(PangaeaBillIngFinder.ShouldSkipThing), new Type[] { typeof(Thing), typeof(int) });
        private static FieldInfo thingDefFieldInfo = AccessTools.Field(typeof(Thing), "def");
        private static MethodInfo requiredCountMethodInfo = AccessTools.Method(typeof(IngredientCount), "CountRequiredOfFor", new Type[] { typeof(ThingDef), typeof(RecipeDef) });

        [HarmonyReversePatch(HarmonyReversePatchType.Original)]
        private static bool ResourceSelectionForBillReversePatch(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> og = VanillaFindIngredientsFix.FixFindIngredientBugs(instructions, out int patchEndIndex, out object skipThingDestination);

                List<CodeInstruction> insert = new List<CodeInstruction>();
                //Get thing from the array
                int stopIndex = -1;
                og.BackUntil(i => og[i].LoadsField(thingDefFieldInfo), patchEndIndex,
                    matchCallback: i => stopIndex = i)
                    .BackUntil(i => og[i].opcode == OpCodes.Br)
                    .ForwardUntil(i => i == stopIndex - 1,
                    action: i => insert.Add(new CodeInstruction(og[i].opcode, og[i].operand)));

                //Get local required amount variable and convert it to int
                og.BackUntil(i => og[i].Calls(requiredCountMethodInfo), patchEndIndex)
                    .ForwardUntil(i => og[i].opcode == OpCodes.Stloc_S,
                    matchCallback: i => insert.AddRange(new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Ldloc_S, og[i].operand),
                        new CodeInstruction(OpCodes.Conv_I4)
                    }));

                //Get the final ShouldSkip bool and, if true, skip this loop
                insert.AddRange(new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Call, shouldSkipResourceMethodInfo),
                        new CodeInstruction(OpCodes.Brtrue, skipThingDestination),
                    });
                int insertIndex = patchEndIndex;
                og.InsertRange(insertIndex, insert);
                return og;
            }

            _ = Transpiler(null);
            return default;
        }
    }
}