#if !RWV13
#define LACKSDRILLERPARAM
#endif

using RimWorld;
using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;


namespace ProjectPangaea
{
    public static class DrillYieldHelper
    {
        public static PangaeaResource GetRandomFossil()
        {
            var fossilDef = ResourceTypeDefOf.Pangaea_Fossil;
            var randomEntry = PangaeaDatabase.AllEntries.Where(e => e.GetResourceOfDef(fossilDef) != null).RandomElement();
            return randomEntry?.GetResourceOfDef(ResourceTypeDefOf.Pangaea_Fossil);
        }
    }

    //TODO: Make changes to the DeepResourceGrid to organize each fossil clump by time period, just like above ground fossil ores
    [HarmonyPatch(typeof(CompDeepDrill), "TryProducePortion")]
    public static class RandomDrillYieldOverride
    {
        private static MethodInfo makeThingMethodInfo = AccessTools.Method(typeof(ThingMaker), nameof(ThingMaker.MakeThing));
        private static MethodInfo randomFossilMethodInfo = AccessTools.Method(typeof(DrillYieldHelper), nameof(DrillYieldHelper.GetRandomFossil));
        private static MethodInfo makeFossilThingMethodInfo = AccessTools.Method(typeof(PangaeaResource), "MakeThing");

        [HarmonyReversePatch(HarmonyReversePatchType.Original)]
#if LACKSDRILLERPARAM
        public static void RandomYieldReversePatch(CompDeepDrill instance, float yieldPct)
#else
        public static void RandomYieldReversePatch(CompDeepDrill instance, float yieldPct, Pawn driller)
#endif
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> og = instructions.ToList();

                int thingPatchStart = -1;
                int thingPatchEnd = -1;

                og.ForwardUntil(i => og[i].Calls(makeThingMethodInfo), matchCallback: i => thingPatchEnd = i)
                    .BackUntil(i => og[i].opcode == OpCodes.Ldloc_0, matchCallback: i => thingPatchStart = i);

                og.RemoveRange(thingPatchStart, thingPatchEnd - thingPatchStart + 1);
                og.InsertRange(thingPatchStart, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, randomFossilMethodInfo),
                    new CodeInstruction(OpCodes.Call, makeFossilThingMethodInfo),
                    //TODO I added this one idk if i should :))
                    new CodeInstruction(OpCodes.Castclass, typeof(Thing))
                });

                return og;
            }

            _ = Transpiler(null);
            return;
        }

        [HarmonyPrefix]
#if LACKSDRILLERPARAM
        public static bool RandomYieldPrefix(CompDeepDrill __instance, float yieldPct)
#else
        public static bool RandomYieldPrefix(CompDeepDrill __instance, float yieldPct, Pawn driller)
#endif
        {
            ThingWithComps drill = __instance.parent;
            ThingDef resource = drill.Map.deepResourceGrid.ThingDefAt(drill.Position);
            if (resource == PangaeaThingDefOf.Pangaea_FossilBase)
            {
#if LACKSDRILLERPARAM
                RandomYieldReversePatch(__instance, yieldPct);
#else
                RandomYieldReversePatch(__instance, yieldPct, driller);
#endif
                return false;
            }
            return false;
        }
    }
}
