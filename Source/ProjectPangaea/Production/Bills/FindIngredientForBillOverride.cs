using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System;

namespace ProjectPangaea.Production
{
    public static class PangaeaBillIngFinder
    {
        private static Dictionary<PangaeaResource, int> availableResourceCountDict = new Dictionary<PangaeaResource, int>();
        private static List<PangaeaResourceThingBase> availableResourceThings = new List<PangaeaResourceThingBase>();
        private static PangaeaResource lastResource = null;

        public static void Clear()
        {
            availableResourceCountDict.Clear();
            availableResourceThings.Clear();
            lastResource = null;
        }

        public static void RegisterThing(PangaeaResourceThingBase pangaeaThing)
        {
            if (pangaeaThing == null || pangaeaThing.Resource == null)
            {
                return;
            }

            availableResourceThings.Add(pangaeaThing);
            if (!availableResourceCountDict.ContainsKey(pangaeaThing.Resource))
            {
                availableResourceCountDict.Add(pangaeaThing.Resource, pangaeaThing.stackCount);
            }
            else
            {
                availableResourceCountDict[pangaeaThing.Resource] += pangaeaThing.stackCount;
            }
        }

        private static bool HasRequiredCount(PangaeaResource resource, int required)
        {
            if (resource == null)
            {
                return false;
            }

            return availableResourceCountDict.TryGetValue(resource, out int count) &&  count >= required;
        }

        private static bool IsSameResourceAsLast(PangaeaResource resource)
        {
            if (resource == null)
            {
                return false;
            }

            return (lastResource == null || resource == lastResource);
        }

        public static bool ShouldSkipThing(Thing thing, int required)
        {
            if (!(thing is PangaeaResourceThingBase pt))
            {
                return false;
            }

            if (pt == null || !HasRequiredCount(pt.Resource, required) || !IsSameResourceAsLast(pt.Resource))
            {
                return true;
            }

            Log.Message($"Wont skip {pt.Resource.Label} while lastResource = {lastResource?.Label ?? "null"}");
            lastResource = pt.Resource;
            return false;
        }

    }


    [HarmonyPatch(typeof(WorkGiver_DoBill), "TryFindBestBillIngredientsInSet_NoMix")]
    public static class FindIngredientForBillOverride
    {
        [HarmonyPrefix]
        public static bool ProcessAvailableThingsPrefix(ref bool __result, List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
        {
            Pangaea_ResourceRecipeExtension resourceExtension = bill.recipe?.GetModExtension<Pangaea_ResourceRecipeExtension>();
            if (resourceExtension == null)
            {
                return true;
            }

            PangaeaBillIngFinder.Clear();
            for (int i = 0; i < availableThings.Count; i++)
            {
                Thing thing = availableThings[i];

                if (thing is PangaeaResourceThingBase pangaeaThing)
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

        //todo make this pretty iterator thing yield return
        [HarmonyDebug]
        [HarmonyReversePatch(HarmonyReversePatchType.Original)]
        public static bool ResourceSelectionForBillReversePatch(List<Thing> availableThings, Bill bill, List<ThingCount> chosen, IntVec3 rootCell, bool alreadySorted)
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> og = instructions.ToList();
                int index = -1;
                og.ForwardUntil(i => og[i].opcode == OpCodes.Bne_Un && og[i - 1].Calls(getDefMethodInfo),
                    matchCallback: i => index = i);
                var continueDestination = og[index].operand;

                List<CodeInstruction> insert = new List<CodeInstruction>();

                //Get thing from the array
                int thingFromArrayStartIndex = index - 1;
                int stopIndex = -1;
                og.BackUntil(i => og[i].LoadsField(thingDefFieldInfo), thingFromArrayStartIndex,
                    matchCallback: i => stopIndex = i)
                    .BackUntil(i => og[i].opcode == OpCodes.Br)
                    .ForwardUntil(i => i == stopIndex - 1,
                    callback: i => insert.Add(new CodeInstruction(og[i].opcode, og[i].operand)));

                //Get local required amount variable
                int requiredCountStartingIndex = index - 1;
                og.BackUntil(i => og[i].Calls(requiredCountMethodInfo), requiredCountStartingIndex)
                    .ForwardUntil(i => og[i].opcode == OpCodes.Stloc_S,
                    matchCallback: i => insert.Add(new CodeInstruction(OpCodes.Ldloc_S, og[i].operand)));

                //Get the final boolean and, if true, continue;
                insert.AddRange(new CodeInstruction[] {
                        new CodeInstruction(OpCodes.Conv_I4),
                        new CodeInstruction(OpCodes.Call, shouldSkipResourceMethodInfo),
                        new CodeInstruction(OpCodes.Brtrue, continueDestination),
                    });

                int insertIndex = index + 1;
                og.InsertRange(insertIndex, insert);
                return og;
            }

            _ = Transpiler(null);
            return default;
        }
    }
}