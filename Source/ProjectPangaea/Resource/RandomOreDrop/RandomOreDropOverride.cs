using RimWorld;
using Verse;
using System.Linq;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ProjectPangaea
{
    [HarmonyPatch(typeof(Mineable), "TrySpawnYield")]
    public static class RandomOreDropOverride
    {
        private static MethodInfo makeThingMethodInfo = AccessTools.Method(typeof(ThingMaker), nameof(ThingMaker.MakeThing));
        private static MethodInfo getOreDropExtMethodInfo = AccessTools.Method(typeof(ThingDef), "GetModExtension", generics: new[]{typeof(ModExt_RandomFossilDrop)});
        private static MethodInfo getDropThingMethodInfo = AccessTools.Method(typeof(ModExt_RandomFossilDrop), "GetRandomDrop");
        private static FieldInfo thingDefFieldInfo = AccessTools.Field(typeof(Thing), "def");
        private static FieldInfo stackCountFieldInfo = AccessTools.Field(typeof(Thing), "stackCount");

        [HarmonyReversePatch(HarmonyReversePatchType.Original)]
        public static void RandomDropReversePatch(Mineable instance, Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> og = instructions.ToList();
                int thingPatchStart = -1;
                int thingPatchEnd = -1;
                int stackPatchStart = -1;
                int stackPatchEnd = -1;

                var makeThing = og.ForwardUntil(i => og[i].Calls(makeThingMethodInfo),
                    matchCallback: i => thingPatchEnd = i);
                
                makeThing.BackUntil(i => og[i].LoadsField(thingDefFieldInfo) && og[i - 1].opcode == OpCodes.Ldarg_0,
                    matchCallback: i => thingPatchStart = i + 1);

                makeThing.ForwardUntil(i => og[i].StoresField(stackCountFieldInfo),
                    matchCallback: i => {
                        stackPatchEnd = i;
                        stackPatchStart = i - 2;
                        });

                og.RemoveRange(stackPatchStart, stackPatchEnd - stackPatchStart + 1);

                og.RemoveRange(thingPatchStart, thingPatchEnd - thingPatchStart + 1);
                og.InsertRange(thingPatchStart, new CodeInstruction[] {
                    new CodeInstruction(OpCodes.Call, getOreDropExtMethodInfo),
                    new CodeInstruction(OpCodes.Call, getDropThingMethodInfo)
                });

                return og;
            }

            _ = Transpiler(null);
            return;
        }

        [HarmonyPrefix]
        public static bool RandomDropPrefix(Mineable __instance, Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
        {
            if (__instance.def.HasModExtension<ModExt_RandomFossilDrop>())
            {
                if (__instance.def.building.mineableThing == null)
                {
                    __instance.def.building.mineableThing = PangaeaThingDefOf.Pangaea_FossilBase; 
                }

                RandomDropReversePatch(__instance, map, yieldChance, moteOnWaste, pawn);
                return false;
            }
            return true;
        }
    }
}
