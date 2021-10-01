using HarmonyLib;
using System;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;
using ProjectPangaea.Production;

namespace ProjectPangaea.PangaeaUI
{
    [HarmonyPatch(typeof(Bill_Production), "DoConfigInterface")]
    public static class BillConfigWindowHarmonyPatches
    {
        //TODO ADD THE WINDOWS
        /*[HarmonyReversePatch]
        public static void Extraction_ChangeBillDetailButton(Bill_Production __instance, Rect baseRect, Color baseColor)
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (CodeInstruction code in instructions)
                {
                    if (code.opcode == OpCodes.Newobj && (ConstructorInfo)code.operand == AccessTools.Constructor(typeof(Dialog_BillConfig), new System.Type[] { typeof(Bill_Production), typeof(IntVec3) }))
                    {
                        code.operand = AccessTools.Constructor(typeof(PangaeaBillConfigWindow), new Type[] { typeof(PangaeaResourceBill), typeof(IntVec3) });
                    }
                    yield return code;
                }
            }
            _ = Transpiler(null);
        }

        [HarmonyPrefix]
        public static bool BillConfigInterfacePrefix(Bill_Production __instance, Rect baseRect, Color baseColor)
        {
            if (__instance.recipe.HasModExtension<Pangaea_ResourceRecipeExtension>())
            {
                Extraction_ChangeBillDetailButton(__instance, baseRect, baseColor);
                return false;
            }
            return true;
        }*/
    }
}
