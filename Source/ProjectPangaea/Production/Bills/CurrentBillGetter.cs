using Verse;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using Verse.AI;

namespace ProjectPangaea.Production
{
    [HarmonyPatch]
    public class CurrentBillGetter
    {
        public static PangaeaBill CurrentBill { get; private set; }

        //'<FinishRecipeAndStartStoringProduct>b__0'
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
            if (___toil.actor.jobs.curJob.bill is PangaeaBill splicingBill)
            {
                CurrentBill = splicingBill;
            }
            return true;
        }
    }
}
