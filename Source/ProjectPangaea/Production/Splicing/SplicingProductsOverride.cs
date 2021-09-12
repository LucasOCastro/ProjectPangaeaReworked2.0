using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using Verse.AI;
using RimWorld;

namespace ProjectPangaea.Production.Splicing
{
    [HarmonyPatch]
    public class SplicingProductsOverride
    {
        public static DNASplicingBill CurrentBill { get; private set; }

        private static MethodBase TargetMethod()
        {
            var method = AccessTools.FindIncludingInnerTypes(typeof(Toils_Recipe),
                    t => AccessTools.FirstMethod(t, m =>
                            m.Name.Contains(nameof(Toils_Recipe.FinishRecipeAndStartStoringProduct))
                            && !m.IsStatic && m.ReturnType == typeof(void)));
            Log.Message("PATCHED THE: " + method.Name);
            return method;
        }

        private static bool Prefix(Toil ___toil)
        {
            Log.Message("THIS WORKS AAA");

            if (___toil.actor.jobs.curJob.bill is DNASplicingBill splicingBill)
            {
                CurrentBill = splicingBill;
            }
            return true;
        }
    }
    //[HarmonyPatch("Verse.AI.Toils_Recipe.<>c__DisplayClass3_0.<FinishRecipeAndStartStoringProduct>b__0")]
    /*[HarmonyPatch]
    public class SplicingProductsOverride
    {
        private static MethodBase TargetMethod()
        {
            var method = AccessTools.FindIncludingInnerTypes(typeof(Toils_Recipe),
                    t => AccessTools.FirstMethod(t, m =>
                            m.Name.Contains(nameof(Toils_Recipe.FinishRecipeAndStartStoringProduct))
                            && !m.IsStatic && m.ReturnType == typeof(void)));
            Log.Message("PATCHED THE: " + method.Name);
            return method;
        }

        private static bool Prefix(Toil ___toil)
        {
            Log.Message("THIS WORKS AAA");

            if (___toil.actor.jobs.curJob.bill is DNASplicingBill splicingBill)
            {
                currentBill = splicingBill;
                ReversePatch();
                return false;
            }
            return true;
        }

        private static DNASplicingBill currentBill = null;
        private static void PostProcessProducts(List<Thing> products)
        {
            products.AddRange(currentBill.MakeResults().ToList());
        }

        //private static MethodInfo makeProductsMethodInfo = AccessTools.Method(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts));
        //private static MethodInfo processProductsMethodInfo = AccessTools.Method(typeof(SplicingProductsOverride), nameof(PostProcessProducts));
        [HarmonyReversePatch]
        private static void ReversePatch()
        {
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return instructions;
                List<CodeInstruction> og = instructions.ToList();

                object productListVar = null;
                var getProductList = og.ForwardUntil(i => og[i].Calls(makeProductsMethodInfo))
                    .ForwardUntil(i => og[i].opcode == OpCodes.Stloc_S,
                    matchCallback: i => productListVar = og[i].operand);

                List<CodeInstruction> insert = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldloc_S, productListVar),
                    new CodeInstruction(OpCodes.Call, processProductsMethodInfo),
                };
                int insertIndex = getProductList.index + 1;
                og.InsertRange(insertIndex, insert);

                return og;
            }

            _ = Transpiler(null);
            return;
        }
    }*/
}
