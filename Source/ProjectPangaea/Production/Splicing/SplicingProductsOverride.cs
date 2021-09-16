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
            return method;
        }

        private static bool Prefix(Toil ___toil)
        {
            if (___toil.actor.jobs.curJob.bill is DNASplicingBill splicingBill)
            {
                CurrentBill = splicingBill;
            }
            return true;
        }
    }
}
